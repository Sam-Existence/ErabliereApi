using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle de modification d'une erablière
/// </summary>
public class PutErabliere
{
    /// <summary>
    /// L'id de l'érablière à modifier.
    /// </summary>
    [Required]
    public Guid? Id { get; set; }

    /// <summary>
    /// Le nouveau nom de l'érablière, si le nom est modifié
    /// </summary>
    [MaxLength(50)]
    public string? Nom { get; set; }

    /// <summary>
    /// Spécifie les ip qui peuvent créer des opérations d'alimentation pour cette érablière. Doivent être séparé par des ';'
    /// </summary>
    [MaxLength(50)]
    public string? IpRule { get; set; }

    /// <summary>
    /// Un indice permettant l'affichage des érablières dans l'ordre précisé.
    /// </summary>
    public int? IndiceOrdre { get; set; }

    /// <summary>
    /// Code postal utilisé pour les prédictions météo
    /// </summary>
    public string? CodePostal { get; set; }

    /// <summary>
    /// Indicateur permettant de déterminer si la section des barils sera utiliser par l'érablière
    /// </summary>
    public bool? AfficherSectionBaril { get; set; }

    /// <summary>
    /// Indicateur permettant de déterminer si la section des donnees sera utiliser par l'érablière
    /// </summary>
    public bool? AfficherTrioDonnees { get; set; }

    /// <summary>
    /// Indicateur permettant de déterminer si la section des donnees sera utiliser par l'érablière
    /// </summary>
    public bool? AfficherSectionDompeux { get; set; }

    /// <summary>
    /// Indiuateur permettant une accès en lecture à l'érablière sans authentifications
    /// </summary>
    public bool? IsPublic { get; set; }
}
