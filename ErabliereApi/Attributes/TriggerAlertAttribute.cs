using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit;
using System.Text;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ErabliereApi.Controllers.Attributes;

/// <summary>
/// Classe qui permet de rechercher et lancer les alertes relier à une action.
/// </summary>
public class TriggerAlertAttribute : ActionFilterAttribute
{
    private Guid? _idErabliere;
    private PostDonnee? _donnee;

    /// <summary>
    /// Contructeur par initialisation.
    /// </summary>
    /// <param name="order">Ordre d'exectuion des action filter</param>
    public TriggerAlertAttribute(int order = int.MinValue)
    {
        Order = order;
    }

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var id = context.ActionArguments["id"]?.ToString() ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlert'.");

        _idErabliere = Guid.Parse(id ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlert'."));

        _donnee = context.ActionArguments.Values.Single(a => a?.GetType() == typeof(PostDonnee)) as PostDonnee;
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

                var emailConfig = context.HttpContext.RequestServices.GetRequiredService<IOptions<EmailConfig>>();

                var alertes = await depot.Alertes.AsNoTracking().Where(a => a.IdErabliere == _idErabliere && a.IsEnable).ToArrayAsync();

                for (int i = 0; i < alertes.Length; i++)
                {
                    var alerte = alertes[i];

                    MaybeTriggerAlerte(alerte, logger, emailConfig.Value);
                }
            }
            catch (Exception e)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TriggerAlertAttribute>>();

                logger.LogCritical(new EventId(92837485, "TriggerAlertAttribute.OnActionExecuted"), e, "Une erreur imprévue est survenu lors de l'execution de la fonction d'alertage.");
            }
        }
    }

    private void MaybeTriggerAlerte(Alerte alerte, ILogger<TriggerAlertAttribute> logger, EmailConfig emailConfig)
    {
        if (_donnee == null)
        {
            throw new InvalidOperationException("La donnée membre '_donnee' doit être initialiser pour utiliser la fonction d'alertage.");
        }

        var validationCount = 0;
        var conditionMet = 0;

        if (alerte.NiveauBassinThresholdHight != null && short.TryParse(alerte.NiveauBassinThresholdHight, out short nbth))
        {
            validationCount++;

            if (nbth > _donnee.NB)
            {
                conditionMet++;
            }
        }

        if (alerte.NiveauBassinThresholdLow != null && short.TryParse(alerte.NiveauBassinThresholdLow, out short nbtl))
        {
            validationCount++;

            if (nbtl < _donnee.NB)
            {
                conditionMet++;
            }
        }

        if (alerte.VacciumThresholdHight != null && short.TryParse(alerte.VacciumThresholdHight, out short vth))
        {
            validationCount++;

            if (vth > _donnee.V)
            {
                conditionMet++;
            }
        }

        if (alerte.VacciumThresholdLow != null && short.TryParse(alerte.VacciumThresholdLow, out short vtl))
        {
            validationCount++;

            if (vtl < _donnee.V)
            {
                conditionMet++;
            }
        }

        if (alerte.TemperatureThresholdHight != null && short.TryParse(alerte.TemperatureThresholdHight, out short tth))
        {
            validationCount++;

            if (tth > _donnee.T)
            {
                conditionMet++;
            }
        }

        if (alerte.TemperatureThresholdLow != null && short.TryParse(alerte.TemperatureThresholdLow, out short ttl))
        {
            validationCount++;

            if (ttl < _donnee.T)
            {
                conditionMet++;
            }
        }

        if (validationCount > 0 && validationCount == conditionMet)
        {
            TriggerAlerte(alerte, logger, emailConfig);
        }
    }

    private async void TriggerAlerte(Alerte alerte, ILogger<TriggerAlertAttribute> logger, EmailConfig emailConfig)
    {
        if (!emailConfig.IsConfigured)
        {
            logger.LogWarning("Les configurations ne courriel ne sont pas initialisé, la fonctionnalité d'alerte ne peut pas fonctionner.");

            return;
        }

        try
        {
            if (alerte.EnvoyerA != null)
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("ErabliereAPI - Alerte Service", emailConfig.Sender));
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
                await smtpClient.ConnectAsync(emailConfig.SmtpServer, emailConfig.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(emailConfig.Email, emailConfig.Password);
                await smtpClient.SendAsync(mailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(new EventId(92837486, "TriggerAlertAttribute.TriggerAlerte"), e, "Une erreur imprévue est survenu lors de l'envoie de l'alerte.");
        }
    }

    private static string FormatTextMessage(Alerte alerte, PostDonnee? donnee)
    {
        var sb = new StringBuilder();

        sb.Append(nameof(Alerte));
        sb.AppendLine(" : ");
        sb.AppendLine(Serialize(alerte, _mailSerializerSettings));
        sb.AppendLine();
        sb.Append(nameof(PostDonnee));
        sb.AppendLine(" : ");
        sb.AppendLine(Serialize(donnee, _mailSerializerSettings));
        sb.AppendLine();
        sb.AppendLine($"Date : {DateTimeOffset.UtcNow}");

        return sb.ToString();
    }

    private readonly static JsonSerializerOptions _mailSerializerSettings = new() { WriteIndented = true };
}
