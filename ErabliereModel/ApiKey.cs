using System;

namespace ErabliereApi.Donnees;

/// <summary>
/// Modèle contenant représentant une clé d'api est l'information relié à cette clé
/// </summary>
public class ApiKey
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// La clé d'api sous forme de hash
    /// </summary>
    public string Key { get; set; } = "";

    /// <summary>
    /// Date de création de la clé
    /// </summary>
    public DateTimeOffset CreationTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Date de révocation de la clé
    /// </summary>
    public DateTimeOffset? RevocationTime { get; set; }

    /// <summary>
    /// Date de suppression de la clé
    /// </summary>
    public DateTimeOffset? DeletionTime { get; set; }

    /// <summary>
    /// L'id du <see cref="Customer"/> possédant la clé d'api
    /// </summary>
    public Guid CustomerId {get;set;}
    
    /// <summary>
    /// Le <see cref="Customer"/> possédant la clé d'api
    /// </summary>
    public Customer? Customer { get; set; }
}
