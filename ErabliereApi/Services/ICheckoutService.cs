namespace ErabliereApi.Services;

/// <summary>
/// 
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    /// 
    /// </summary>
    Task<object> CreateSessionAsync();

    /// <summary>
    /// 
    /// </summary>
    Task Webhook(string json);
}
