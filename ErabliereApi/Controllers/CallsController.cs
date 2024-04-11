using ErabliereApi.Services.Agora.Recording.Stop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des dompeux
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "ErabliereCalls", Policy = "TenantIdPrincipal")]
public class CallsController : ControllerBase
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Calls controller, allow users to call each other
    /// </summary>
    public CallsController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Get the appId use to initialize client
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAppId(CancellationToken token)
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var tokenResponse = await client.GetAsync($"{localBackend}/accessToken/appId/view", token);

        return Ok(await tokenResponse.Content.ReadFromJsonAsync(typeof(object), token));
    }


    /// <summary>
    /// Get an access token
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="channel"></param>
    /// <param name="role"></param>
    /// <param name="tokenType"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAccessToken([FromQuery] int? uid, [FromQuery] string? channel, [FromQuery] string? role, [FromQuery] string? tokenType, CancellationToken token)
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var tokenResponse = await client.GetAsync($"{localBackend}/accessToken/{uid}?channel={channel}&tokenType={tokenType}", token);

        return Ok(await tokenResponse.Content.ReadFromJsonAsync(typeof(object), token));
    }

    /// <summary>
    /// Starts the recording of call
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> StartRecording([FromQuery] int uid, [FromQuery] string channel, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var resourceIdResponse = await client.GetAsync($"{localBackend}/recording/acquire/{channel}?uid={uid}", cancellationToken);

        var resourceId = resourceIdResponse.Content.ReadFromJsonAsync(typeof(string), cancellationToken);

        var recordingResponse = await client.GetAsync($"{localBackend}/recording/start/{channel}?uid={uid}&resourceId={resourceId}");

        return Ok(recordingResponse.Content.ReadFromJsonAsync(typeof(object), cancellationToken));
    }

    /// <summary>
    /// Stops the recording of call
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> StopRecording(
        [FromQuery] int uid, 
        [FromQuery] string channel, 
        [FromQuery] string resourceId, 
        [FromQuery] string sid,
        CancellationToken cancellationToken
    )
    {
        using var client = new HttpClient();

        var localBackend = _config["agora.localbackend"]?.Trim();

        if (localBackend != null && localBackend.EndsWith('/'))
        {
            localBackend = localBackend.Remove(localBackend.Length - 1);
        }

        var stopResponse = await client.GetAsync($"{localBackend}/recording/stop/{channel}?uid={uid}&resourceId={resourceId}&sid={sid}");

        return Ok(stopResponse.Content.ReadFromJsonAsync(typeof(Reponse), cancellationToken));
    }
}
