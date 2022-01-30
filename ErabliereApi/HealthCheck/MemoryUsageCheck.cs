using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System.Text;

namespace ErabliereApi.HealthCheck;

/// <summary>
/// Health check permettant d'indiquer si trop de mémoire est utilisé par l'api
/// </summary>
public class MemoryUsageCheck : IHealthCheck
{
    const string MetricWatch = "dotnet_total_memory_bytes";
    const long MaxMemoryUsage = 500_000_000;
    private readonly CollectorRegistry _registry;

    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="registry"></param>
    public MemoryUsageCheck(CollectorRegistry registry)
    {
        _registry = registry;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();

        await _registry.CollectAndExportAsTextAsync(stream, cancellationToken);

        stream.Position = 0;

        // convert the stream to a string
        using var reader = new StreamReader(stream);

        var text = reader.ReadToEnd();

        var metricLine = text?.Split('\n').FirstOrDefault(s => s.StartsWith(MetricWatch));

        if (metricLine != null)
        {
            var memory = ParseLong(metricLine);

            if (memory > MaxMemoryUsage)
            {
                return HealthCheckResult.Unhealthy("Memory usage is too high");
            }
        }

        return HealthCheckResult.Healthy("Memory is good");
    }

    private static long ParseLong(string metricLine)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < metricLine.Length; i++)
        {
            if (char.IsDigit(metricLine[i]))
            {
                sb.Append(metricLine[i]);
            }
        }

        return long.Parse(sb.ToString());
    }
}
