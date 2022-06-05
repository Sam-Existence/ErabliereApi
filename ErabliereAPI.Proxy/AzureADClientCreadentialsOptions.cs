namespace ErabliereAPI.Proxy;

public class AzureADClientCreadentialsOptions
{
    public string Host { get; set; } = "login.microsoftonline.com";

    public string PathSuffix { get; set; } = "/oauth2/v2.0/token";

    public string TenantId { get; set; } = "";

    public string ClientId { get; set; } = "";

    public string ClientSecret { get; set; } = "";

    public Uri? Uri 
    { 
        get
        {
            return new Uri($"https://{Host}/{TenantId}{PathSuffix}");
        }
    }

    public string Scope { get; set; } = "";
}