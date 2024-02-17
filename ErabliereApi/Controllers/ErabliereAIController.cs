using AutoMapper;
using Azure;
using Azure.AI.OpenAI;
using ErabliereApi.Depot.Sql;
using ErabliereModel.Action.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des dompeux
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "ErabliereAIUser")]
public class ErabliereAIController : ControllerBase 
{
    private readonly ErabliereDbContext _depot;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="depot"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    public ErabliereAIController(ErabliereDbContext depot, IMapper mapper, IConfiguration configuration)
    {
        _depot = depot;
        _mapper = mapper;
        _configuration = configuration;
    }

    /// <summary>
    /// Liste les conversation
    /// </summary>
    [HttpGet("Conversations")]
    [EnableQuery]
    public IActionResult GetConversation()
    {
        // conversation should be filtered by the user
        var userSub = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        return Ok(_depot.Conversations.Where(c => c.UserId == userSub));
    }

    /// <summary>
    /// Liste les messages
    /// </summary>
    [HttpGet("Conversations/{id}/Messages")]
    [EnableQuery]
    public IActionResult GetMessages(Guid conversationId)
    {
        // conversation should be filtered by the user
        var userSub = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

#nullable disable
        return Ok(_depot.Messages.Where(m => m.ConversationId == conversationId && 
                                             m.Conversation.UserId == userSub));    
#nullable enable             
    }

    /// <summary>
    /// Envoyer un prompt à l'IA
    /// </summary>
    [HttpPost("Prompt")]
    [ProducesResponseType(200, Type = typeof(PostPromptResponse))]
    public async Task<IActionResult> EnvoyerPrompt([FromBody] PostPrompt prompt, CancellationToken cancellationToken)
    {
        // Premièrement ont obtient la conversation
        // if the convesation id is null, create a new conversation
        Conversation? conversation = null;
        if (prompt.ConversationId == null)
        {
            conversation = new Conversation
            {
                UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                CreatedOn = DateTime.Now,
                LastMessageDate = DateTime.Now,
                Name = prompt.Prompt
            };
            _depot.Conversations.Add(conversation);
            await _depot.SaveChangesAsync(cancellationToken);
            prompt.ConversationId = conversation.Id;
        }
        else 
        {
            conversation = await _depot.Conversations.FindAsync([prompt.ConversationId], cancellationToken);

            if (conversation != null) 
            {
                conversation.LastMessageDate = DateTime.Now;
            }
        }

        // Ensuite ont envoie le prompt à l'IA
        var client = new OpenAIClient(
            new Uri(_configuration["AzureOpenAIUri"] ?? ""),
            new AzureKeyCredential(_configuration["AzureOpenAIKey"] ?? "")
        );

        string aiResponse = "No response";

        switch (prompt.PromptType)
        {
            case "Chat":
                // Dans le prompt de type Chat, on obtient l'historique de la conversation
                var messages = await _depot.Messages
                    .Where(m => m.ConversationId == prompt.ConversationId)
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync(cancellationToken);

                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0
                };

                chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.System, "Vous êtes un acériculteur expérimenté avec des connaissance scientifique et pratique."));

                foreach (var message in messages)
                {
                    chatCompletionsOptions.Messages.Add(new ChatMessage(message.IsUser ? ChatRole.User : ChatRole.Assistant, message.Content));
                }

                chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, prompt.Prompt));

                Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
                    _configuration["AzureOpenAIDeploymentChatModelName"],
                    chatCompletionsOptions
                );

                ChatCompletions responseChat = responseWithoutStream.Value;
                aiResponse = responseChat?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response";
                break;
            default:
                var completionResponse = await client.GetCompletionsAsync(
                    deploymentOrModelName: _configuration["AzureOpenAIDeploymentModelName"],
                    new CompletionsOptions
                    {
                        Prompts = { prompt.Prompt },
                        Temperature = (float)1,
                        MaxTokens = 800,
                        NucleusSamplingFactor = (float)0.5,
                        FrequencyPenalty = (float)0,
                        PresencePenalty = (float)0,
                        GenerationSampleCount = 1,
                    }
                );
                var completion = completionResponse.Value;

                var localText = completion?.Choices?.FirstOrDefault()?.Text;
                if (localText != null) {
                    aiResponse = localText;
                }
                break;
        }

        // create the messages for the database
        var query = new Message
        {
            ConversationId = prompt.ConversationId,
            Content = prompt.Prompt ?? "",
            IsUser = true,
            CreatedAt = DateTime.Now,
        };

        var response = new Message
        {
            ConversationId = prompt.ConversationId,
            Content = aiResponse,
            IsUser = false,
            CreatedAt = DateTime.Now,
        };

        await _depot.Messages.AddAsync(query);
        await _depot.Messages.AddAsync(response);
        await _depot.SaveChangesAsync(cancellationToken);

        return Ok(new PostPromptResponse 
        {
            Prompt = prompt,
            Conversation = conversation,
            Response = response,
        });
    }

    /// <summary>
    /// Delete a conversation
    /// </summary>
    [HttpDelete("Conversations/{id}")]
    public async Task<IActionResult> DeleteConversation(Guid id, CancellationToken cancellationToken)
    {
        var conversation = await _depot.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (conversation == null)
        {
            return NoContent();
        }

        _depot.Conversations.Remove(conversation);
        await _depot.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}