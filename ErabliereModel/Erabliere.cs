using ErabliereApi.Donnees.Ownable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ErabliereApi.Donnees;

/// <summary>
/// Modèle représentant une érablière
/// </summary>
public class Erabliere : IIdentifiable<Guid?, Erabliere>, IUserOwnable
{
    /// <summary>
    /// L'id de l'érablière
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Le nom de l'érablière
    /// </summary>
    [Required(ErrorMessage = "Le nom de l'érablière est requis.")]
    [MaxLength(50, ErrorMessage = "Le nom de l'érablière ne peut pas dépasser 50 caractères.")]
    public string? Nom { get; set; }

    /// <summary>
    /// Addresse IP alloué à faire des opération d'écriture
    /// </summary>
    [MaxLength(50, ErrorMessage = "L'adresse IP ne peut pas dépasser 50 caractères.")]
    public string? IpRule { get; set; }

    /// <summary>
    /// Un indice permettant l'affichage des érablières dans l'ordre précisé.
    /// </summary>
    public int? IndiceOrdre { get; set; }

    /// <summary>
    /// Code postal, utiliser pour les fonctions de prédiction météo
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le code postal ne peut pas dépasser 30 caractères.")]
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
    /// Indique si une érablière est publique ou une authentification est requise
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// Les capteurs de l'érablière
    /// </summary>
    public List<Capteur>? Capteurs { get; set; }

    /// <summary>
    /// Les capteurs d'images de l'érablière
    /// </summary>
    public List<CapteurImage>? CapteursImage { get; set; }

    /// <summary>
    /// Les données relier à l'érablière
    /// </summary>
    public List<Donnee>? Donnees { get; set; }

    /// <summary>
    /// La liste des barils de l'érablière
    /// </summary>
    public List<Baril>? Barils { get; set; }

    /// <summary>
    /// La liste des dompeux de l'érablière
    /// </summary>
    public List<Dompeux>? Dompeux { get; set; }

    /// <summary>
    /// La liste des notes
    /// </summary>
    public List<Note>? Notes { get; set; }

    /// <summary>
    /// La liste des documentations
    /// </summary>
    public List<Documentation>? Documentations { get; set; }

    /// <summary>
    /// Liste des alertes de type trio de données relié à l'érablière
    /// </summary>
    public List<Alerte>? Alertes { get; set; }

    /// <summary>
    /// Liste de jonction entre l'utilisateurs et ses érablières
    /// </summary>
    public List<CustomerErabliere>? CustomerErablieres { get; set; }

    /// <inheritdoc />
    public int CompareTo([AllowNull] Erabliere other)
    {
        if (IndiceOrdre.HasValue && other?.IndiceOrdre == null)
        {
            return -1;
        }
        else if (other?.IndiceOrdre.HasValue == true && IndiceOrdre.HasValue == false)
        {
            return 1;
        }
        else if (IndiceOrdre.HasValue && other?.IndiceOrdre.HasValue == true)
        {
            return IndiceOrdre.Value.CompareTo(IndiceOrdre);
        }

        return string.Compare(Nom, other?.Nom);
    }
}
