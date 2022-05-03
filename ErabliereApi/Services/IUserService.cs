using ErabliereApi.Donnees;

namespace ErabliereApi.StripeIntegration;

public interface IUserService
{
    Task CreateCustomerAsync(Customer customer, CancellationToken token);
}