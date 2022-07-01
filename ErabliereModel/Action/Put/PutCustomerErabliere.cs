using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle de l'interaction PUT /erablieres/{id}/
/// </summary>
public class PutCustomerErabliere
{
    /// <summary>
    /// Id de l'érablière
    /// </summary>
    public Guid IdErabliere { get; set; }

    /// <summary>
    /// Liste des accès des utilisateurs
    /// </summary>
    public List<PutSingleCustomerErabliere>? CustomerErablieres { get; set; }
}

/// <summary>
/// Une opération de droit d'accès
/// </summary>
public class PutSingleCustomerErabliere
{
    /// <summary>
    /// L'action effectué
    /// </summary>
    public PutSingleCustomerErabliereAction Action { get; set; }

    /// <summary>
    /// L'id de l'utilisateur
    /// </summary>
    public Guid IdCustomer { get; set; }

    /// <summary>
    /// Le byte des accès. Voir la propriété <see cref="Donnees.CustomerErabliere.Access"/> pour plus d'information.
    /// </summary>
    public byte Access { get; set; }
}

/// <summary>
/// Enum des actions possible effectué par l'utilisateur pour une édition
/// </summary>
public enum PutSingleCustomerErabliereAction
{
    /// <summary>
    /// Créer un droit d'accès
    /// </summary>
    Create,

    /// <summary>
    /// Modifier un droit d'accès
    /// </summary>
    Edit,

    /// <summary>
    /// Supprimer un droit d'accès
    /// </summary>
    Delete
}