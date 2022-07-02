using System;

namespace ErabliereApi.Donnees.Action.Get;

/// <summary>
/// Modèle de retour des droits d'accès des utilisateurs
/// </summary>
public class GetCustomerAccess
{
    /// <summary>
    /// Clé primaire de la jonction
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

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