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
        var id = context.ActionArguments["id"]?.ToString() ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlertV2'.");

        _idCapteur = Guid.Parse(id ?? throw new InvalidOperationException("Le paramètre Id est requis dans la route pour utiliser l'attribue 'TriggerAlertV2'."));

        try
        {
            _donnee = context.ActionArguments.Values.Single(a => a?.GetType() == typeof(PostDonneeCapteur)) as PostDonneeCapteur;
        }
        catch (InvalidOperationException e)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TriggerAlertV2Attribute>>();

            logger.LogCritical(92837485, e, "typeof(PostDonneeCapteur) not found in {0}", Serialize(context.ActionArguments));

            throw;
        }
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

                var alertes = await depot.AlerteCapteurs.AsNoTracking().Where(a => a.IdCapteur == _idCapteur && a.IsEnable).ToArrayAsync();

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

    private void MaybeTriggerAlerte(AlerteCapteur alerte, ILogger<TriggerAlertAttribute> logger, EmailConfig emailConfig)
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

            if (_donnee.V <= alerte.MinVaue.Value)
            {
                conditionMet++;
            }
        }

        if (alerte.MaxValue.HasValue)
        {
            validationCount++;

            if (_donnee.V >= alerte.MaxValue.Value)
            {
                conditionMet++;
            }
        }

        if (validationCount > 0 && validationCount == conditionMet)
        {
            TriggerAlerte(alerte, logger, emailConfig);
        }
    }

    private async void TriggerAlerte(AlerteCapteur alerte, ILogger<TriggerAlertAttribute> logger, EmailConfig emailConfig)
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
