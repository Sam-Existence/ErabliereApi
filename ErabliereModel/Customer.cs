using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees;

/// <summary>
/// Classe représentant un client de l'api
/// </summary>
public class Customer
{
    /// <summary>
    /// la clé primaire pour identifié l'utilisateur
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Le nom donnée à l'utilisateur
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Un courriel pour identifier l'utilisateur
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Un courriel secondaire relié à l'utilisateur
    /// </summary>
    public string? SecondaryEmail { get; set; }

    /// <summary>
    /// Le type de compte, par exemple Stripe, AzureAD, ou autre.
    /// </summary>
    public string AccountType { get; set; } = "";

    /// <summary>
    /// L'id stripe
    /// </summary>
    public string StripeId { get; set; } = "";

    /// <summary>
    /// Un url d'un compte externe relié à l'utilisateur
    /// </summary>
    public string? ExternalAccountUrl { get; set; }

    /// <summary>
    /// La date de création de l'utilisateur
    /// </summary>
    public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
    
    /// <summary>
    /// La liste des clés d'api de l'utilisateur
    /// </summary>
    public List<ApiKey>? ApiKeys { get; set; }
}
