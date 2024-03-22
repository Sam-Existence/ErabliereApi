using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle pour la modification d'une alerte d'un capteur
/// </summary>
public class PutAlerteCapteur
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    [Required]
    public Guid? Id { get; set; }

    /// <summary>
    /// L'id du capteur
    /// </summary>
    [Required]
    public Guid? IdCapteur { get; set; }

    /// <summary>
    /// Le nom de l'alerte
    /// </summary>
    [MaxLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Une liste d'adresse email séparer par des ';'
    /// </summary>
    /// <example>exemple@courriel.com;exemple2@courriel.com</example>
    [MaxLength(200)]
    public string? EnvoyerA { get; set; }

    /// <summary>
    /// Une liste de numéros de téléphone séparés par des ';'
    /// </summary>
    /// <example>+14375327599;+15749375019</example>
    [MaxLength(200)]
    public string? TexterA { get; set; }

    /// <summary>
    /// La valeur minimal de ce capteur
    /// </summary>
    public short? MinVaue { get; set; }

    /// <summary>
    /// La valeur maximal de ce capteur
    /// </summary>
    public short? MaxValue { get; set; }

    /// <summary>
    /// Indique si l'alerte est activé
    /// </summary>
    public bool IsEnable { get; set; }
}
