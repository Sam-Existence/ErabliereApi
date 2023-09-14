using System;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Classe permettant de modifier les accès d'un utilisateur
/// </summary>
public class PutAdminCustomerAccess 
{
    /// <summary>
    /// Identifiant de l'utilisateur
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifiant de l'érablière
    /// </summary>
    public Guid IdErabliere { get; set; }

    /// <summary>
    /// Niveau d'accès de l'utilisateur
    /// </summary>
    public byte CustomerAccessLevel { get; set; }
}