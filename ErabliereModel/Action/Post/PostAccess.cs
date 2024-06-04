using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post;

/// <summary>
/// Modèle de l'interaction PUT /Erablieres/{id}/Acces
/// </summary>
public class PostAccess
{
    /// <summary>
    /// Le byte des accès. Voir la propriété <see cref="CustomerErabliere.Access"/> pour plus d'information.
    /// </summary>
    [Required(ErrorMessage = "L'accès du client ne peut pas être vide")]
    [Range(0, 15, ErrorMessage = "L'accès du client doit être compris entre 0 et 15")]
    public byte? Access { get; set; }
}