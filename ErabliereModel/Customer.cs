using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees;

public class Customer
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string? SecondaryEmail { get; set; }

    public string AccountType { get; set; }

    public string StripeId { get; set; }

    public string? ExternalAccountUrl { get; set; }

    public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
    
    public List<ApiKey>? ApiKeys { get; set; }
}
