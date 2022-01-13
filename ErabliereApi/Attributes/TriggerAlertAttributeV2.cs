using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit;
using System.Text;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;
using static System.Environment;
using static System.IO.File;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers.Attributes;

/// <summary>
/// Classe qui permet de rechercher et lancer les alertes relier à une action.
/// </summary>
public class TriggerAlertV2Attribute : ActionFilterAttribute
{
    private Guid? _idCapteur;
    private PostDonneeCapteur? _donnee;

    /// <summary>
    /// Contructeur par initialisation.
    /// </summary>
    /// <param name="order">Ordre d'exectuion des action filter</param>
    public TriggerAlertV2Attribute(int order = int.MinValue)
    {
        Order = order;
    }

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var id = context.ActionArguments["id"]?.ToString() ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlert'.");

        _idCapteur = Guid.Parse(id ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlert'."));

        _donnee = context.ActionArguments.Values.Single(a => a?.GetType() == typeof(PostDonneeCapteur)) as PostDonneeCapteur;
    }

    /// <inheritdoc />
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var result = await next();

        if (result.Canceled == false)
        {
            try
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TriggerAlertAttribute>>();

                var depot = context.HttpContext.RequestServices.GetRequiredService<ErabliereDbContext>();

                var alertes = await depot.AlerteCapteurs.AsNoTracking().Where(a => a.IdCapteur == _idCapteur).ToArrayAsync();

                for (int i = 0; i < alertes.Length; i++)
                {
                    var alerte = alertes[i];

                    MaybeTriggerAlerte(alerte, logger);
                }
            }
            catch (Exception e)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TriggerAlertAttribute>>();

                logger.LogCritical(new EventId(92837485, "TriggerAlertAttribute.OnActionExecuted"), e, "Une erreur imprévue est survenu lors de l'execution de la fonction d'alertage.");
            }
        }
    }

    private void MaybeTriggerAlerte(AlerteCapteur alerte, ILogger<TriggerAlertAttribute> logger)
    {
        if (_donnee == null)
        {
            throw new InvalidOperationException("La donnée membre '_donnee' doit être initialiser pour utiliser la fonction d'alertage.");
        }

        var validationCount = 0;
        var conditionMet = 0;

        if (alerte.MinVaue.HasValue)
        {
            validationCount++;

            if (alerte.MinVaue.Value <= _donnee.V)
            {
                conditionMet++;
            }
        }

        if (alerte.MaxValue.HasValue)
        {
            validationCount++;

            if (alerte.MaxValue.Value >= _donnee.V)
            {
                conditionMet++;
            }
        }

        if (validationCount > 0 && validationCount == conditionMet)
        {
            TriggerAlerte(alerte, logger);
        }
    }

    private static readonly EmailConfig? _emailConfig = TryDeserializeEmailConfig();

    /// <summary>
    /// Fonction utilisé pour désérialiser les configurations permettant l'envoie de courriel
    /// </summary>
    /// <returns></returns>
    private static EmailConfig? TryDeserializeEmailConfig()
    {
        var path = GetEnvironmentVariable("EMAIL_CONFIG_PATH");

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("La variable d'environment 'EMAIL_CONFIG_PATH' ne possédant pas de valeur, les configurations de courriel ne seront pas désérialisé.");
        }
        else
        {
            try
            {
                var v = ReadAllText(path);

                return Deserialize<EmailConfig>(v);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Erreur en désérialisant les configurations de l'email. La fonctionnalité des alertes ne pourra pas être utilisé.");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        return default;
    }

    private async void TriggerAlerte(AlerteCapteur alerte, ILogger<TriggerAlertAttribute> logger)
    {
        if (_emailConfig == null)
        {
            logger.LogWarning("Les configurations du courriel sont null, la fonctionnalité d'alerte ne peut pas fonctionner.");

            return;
        }

        try
        {
            if (alerte.EnvoyerA != null)
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("ErabliereAPI - Alerte Service", _emailConfig.Sender));
                foreach (var destinataire in alerte.EnvoyerA.Split(';'))
                {
                    mailMessage.To.Add(MailboxAddress.Parse(destinataire));
                }
                mailMessage.Subject = $"Alerte ID : {alerte.Id}";
                mailMessage.Body = new TextPart("plain")
                {
                    Text = FormatTextMessage(alerte, _donnee)
                };

                using var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_emailConfig.Email, _emailConfig.Password);
                await smtpClient.SendAsync(mailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(new EventId(92837486, "TriggerAlertV2Attribute.TriggerAlerte"), e, "Une erreur imprévue est survenu lors de l'envoie de l'alerte.");
        }
    }

    private static string FormatTextMessage(AlerteCapteur alerte, PostDonneeCapteur? donnee)
    {
        var sb = new StringBuilder();

        sb.Append(nameof(AlerteCapteur));
        sb.AppendLine(" : ");
        sb.AppendLine(Serialize(alerte, _mailSerializerSettings));
        sb.AppendLine();
        sb.Append(nameof(PostDonneeCapteur));
        sb.AppendLine(" : ");
        sb.AppendLine(Serialize(donnee, _mailSerializerSettings));
        sb.AppendLine();
        sb.AppendLine($"Date : {DateTimeOffset.UtcNow}");

        return sb.ToString();
    }

    private readonly static JsonSerializerOptions _mailSerializerSettings = new() { WriteIndented = true };
}
