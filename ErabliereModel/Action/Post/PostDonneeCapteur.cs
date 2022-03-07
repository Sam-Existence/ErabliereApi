using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post;

/// <summary>
/// Modèle d'ajout d'une donnée d'un capteur
/// </summary>
public class PostDonneeCapteur
{
    /// <summary>
    /// La valeur
    /// </summary>
    public short V { get; set; }

    /// <summary>
    /// Text associé à la donnée
    /// </summary>
    [MaxLength(50)]
    public string? Text { get; set; }

    /// <summary>
    /// La date
    /// </summary>
    public DateTimeOffset? D { get; set; }

    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid? IdCapteur { get; set; }
}
