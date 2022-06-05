namespace ErabliereAPI.Proxy;

public class ErabliereApiOptions
{
    public string BaseUrl { get; set; } = "";

    public string TenantId { get; set; } = "";

    public string ClientId { get; set; } = "";

    public string ClientSecret { get; set; } = "";

    public string Scope { get; set; } = "";
}