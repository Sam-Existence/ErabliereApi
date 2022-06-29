using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees.Action.NonHttp;

/// <summary>
/// Classe utilisé pour la projection de la requête Bd pour obtenir les accès
/// </summary>
public class CustomerOwnershipAccess
{
    /// <summary>
    /// la clé primaire pour identifié l'utilisateur
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Nom unique permettant de trouver l'utilisateur basé sur les claims d'un jeton bearer
    /// possédant un claim 'unique_name'
    /// </summary>
    public string UniqueName { get; set; } = "";

    /// <summary>
    /// Liste de jonction entre l'utilisateurs et ses érablières
    /// </summary>
    public List<CustomerErabliereOwnershipAccess> CustomerErablieres { get; set; } = new ();
}
