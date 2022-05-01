using System;

namespace ErabliereApi.Donnees;

public class ApiKey
{
    public Guid? Id { get; set; }

    public string Key { get; set; }

    public DateTimeOffset CreationTime { get; set; } = DateTime.Now;

    public DateTimeOffset? RevocationTime { get; set; }

    public DateTimeOffset? DeletionTime { get; set; }

    public Guid CustomerId {get;set;}
    
    public Customer Customer { get; set; }
}
