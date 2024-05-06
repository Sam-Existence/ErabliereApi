using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Modèle de retour des droits d'accès des utilisateurs
/// </summary>
public class GetCustomerAccess
{
    /// <summary>
    /// Id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// Information sur l'érablière
    /// </summary>
    public GetCustomerAccessErabliere? Erabliere { get; set; }

    /// <summary>
    /// Id de l'utilisateur
    /// </summary>
    public Guid? IdCustomer { get; set; }

    /// <summary>
    /// Information sur l'utilisateur
    /// </summary>
    public GetCustomerAccessCustomer? Customer { get; set; }

    /// <inheritdoc cref="Donnees.CustomerErabliere.Access" />
    public byte Access { get; set; }
}

/// <summary>
/// A customer inside a <see cref="GetCustomerAccess"/>
/// </summary>
public class GetCustomerAccessCustomer
{
    /// <summary>
    /// Le nom donnée à l'utilisateur
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Nom unique permettant de trouver l'utilisateur basé sur les claims d'un jeton bearer
    /// possédant un claim 'unique_name'
    /// </summary>
    public string UniqueName { get; set; } = "";

    /// <summary>
    /// Un courriel pour identifier l'utilisateur
    /// </summary>
    public string Email { get; set; } = "";
}

/// <summary>
/// An érbalière à l'intérieur d'un <see cref="GetCustomerAccess"/>
/// </summary>
public class GetCustomerAccessErabliere
{
    /// <summary>
    /// Le nom de l'érablière
    /// </summary>
    [MaxLength(50)]
    [Required(ErrorMessage = "Le nom de l'érablière ne peut pas être vide.")]
    public string? Nom { get; set; }
}