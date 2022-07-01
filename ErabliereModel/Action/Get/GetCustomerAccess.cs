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

    /// <inheritdoc cref="Donnees.CustomerErabliere.Access" />
    public byte Access { get; set; }
}
