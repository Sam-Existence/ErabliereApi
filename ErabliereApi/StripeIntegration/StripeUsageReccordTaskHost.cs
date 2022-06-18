using ErabliereApi;
using Microsoft.Extensions.Options;
using Stripe;

/// <summary>
/// Une class contenant une méthode d'extension pour décorer l'IHost
/// </summary>
public static class StripeUsageReccordTaskHostExtensions
{
    /// <summary>
    /// Décorateur permettant d'ajouter une tâche de d'envoie de l'utilisation
    /// lorsque Stripe est activé avec la variable d'environnement Stripe.ApiKey
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IHost UseStripeUsageReccordTask(this IHost host)
    {
        var config = host.Services.GetRequiredService<IConfiguration>();

        if (!string.IsNullOrWhiteSpace(config["Stripe.ApiKey"]))
        {
            return new StripeUsageReccordTaskHost(host);
        }

        return host;
    }
}

/// <summary>
/// Décorateur de IHost ajoutant une tâche en arrière pour envoyer l'utilisation à Stripe.
/// </summary>
public class StripeUsageReccordTaskHost : IHost 
{
    private readonly IHost _host;
    private Task? _task;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="StripeUsageReccordTaskHost"/>.
    /// </summary>
    public StripeUsageReccordTaskHost(IHost host)
    {
        _host = host;
    }

    /// <summary>
    /// Services
    /// </summary>
    public IServiceProvider Services => _host.Services;

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _host.Dispose();
    }

    /// <summary>
    /// Démarre l'host
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _task = Task.Run(async () => 
        {
            var options = Services.GetRequiredService<IOptions<StripeOptions>>();

            while (!cancellationToken.IsCancellationRequested) 
            {
                await Task.Delay(options.Value.TimeSpanSendUsage, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    await EnvoyerUtilisationAsync();
                }
            }
        });

        return _host.StartAsync(cancellationToken);
    }

    private async Task EnvoyerUtilisationAsync()
    {
        using (var scope = Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<UsageContext>();

            var usageSummary = new Dictionary<string, UsageRecordCreateOptions>(context.Usages.Count);

            while (context.Usages.TryDequeue(out var usageReccorded))
            {
                if (usageSummary.TryGetValue(usageReccorded.SubscriptionId, out var usage))
                {
                    usage.Quantity += usageReccorded.Quantite;
                }
                else
                {
                    usageSummary.Add(usageReccorded.SubscriptionId, new UsageRecordCreateOptions
                    {
                        Quantity = usageReccorded.Quantite,
                        Timestamp = DateTimeOffset.Now.UtcDateTime
                    });
                }
            }

            var options = scope.ServiceProvider.GetRequiredService<IOptions<StripeOptions>>();

            StripeConfiguration.ApiKey = options.Value.ApiKey;

            foreach (var usageReccord in usageSummary)
            {
                var service = new UsageRecordService();
                await service.CreateAsync(usageReccord.Key, usageReccord.Value);
            }
        }
    }

    /// <summary>
    /// Arrête l'host
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await EnvoyerUtilisationAsync();

        await _host.StopAsync(cancellationToken);
    }
}