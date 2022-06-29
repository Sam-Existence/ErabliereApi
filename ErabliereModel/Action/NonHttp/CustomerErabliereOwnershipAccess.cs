using System;

namespace ErabliereApi.Donnees.Action.NonHttp;

/// <summary>
/// Classe utilisé pour la projection afin d'obtenir l'information sur les accès
/// </summary>
public class CustomerErabliereOwnershipAccess
{
    /// <summary>
    /// Clé primaire de la jonction
    /// </summary>
    public Guid? Id { get; set; }

    /// <inheritdoc cref="Donnees.CustomerErabliere.Access" />
    public byte Access { get; set; }
}
