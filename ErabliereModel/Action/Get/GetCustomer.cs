using System;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Classe d'extension pour limiter le nombre d'informations exposées.
/// </summary>
public class GetCustomer
{
    /// <summary>
    /// Identifiant du client.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Nom du client.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Un nom unique pour le client
    /// </summary>
    public string? UniqueName { get; set; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    public string? Email { get; set; }
}