using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ErabliereAPI.Proxy;

public class AzureADClientCredentialsHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private readonly IOptions<AzureADClientCreadentialsOptions> _options;

    public AzureADClientCredentialsHandler(IMemoryCache cache, IOptions<AzureADClientCreadentialsOptions> options) : base()
    {
        _cache = cache;
        _options = options;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetAzureADToken(cancellationToken);

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async ValueTask<string?> GetAzureADToken(CancellationToken token)
    {
        const string key = "ErabliereAPI-BearerToken";

        if (_cache.TryGetValue(key, out BearerToken bearerToken) && !bearerToken.WillExpireInTwoMinutes)
        {
            return bearerToken.access_token;
        }

        using var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, _options.Value.Uri)
        {
            Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new ("client_id", _options.Value.ClientId),
                new ("scope", _options.Value.Scope),
                new ("client_secret", _options.Value.ClientSecret),
                new ("grant_type", "client_credentials")
            })
        };

        var response = await client.SendAsync(request, token);

        response.EnsureSuccessStatusCode();

        var streamContent = await response.Content.ReadAsStreamAsync(token);

        var content = JsonSerializer.Deserialize<BearerToken>(streamContent);

        _cache.Set(key, content);

        return content?.access_token;
    }
}