namespace ErabliereApi.Services;

public interface ICheckoutService
{
    Task<object> CreateSessionAsync();
}
