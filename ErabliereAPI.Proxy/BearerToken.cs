using System.Text.Json.Serialization;

namespace ErabliereAPI.Proxy;

public class BearerToken
{
    public BearerToken()
    {
        CreationTime = DateTime.Now;
    }

    public string? token_type { get; set; }

    public long expires_in { get; set; }

    [JsonIgnore]
    public DateTime CreationTime { get; set; }

    [JsonIgnore]
    public bool IsExpired => CreationTime + TimeSpan.FromSeconds(expires_in) <= DateTime.Now;

    [JsonIgnore]
    public bool WillExpireInTwoMinutes => CreationTime + TimeSpan.FromSeconds(expires_in - 120) <= DateTime.Now;

    public string? access_token { get; set; }
}
