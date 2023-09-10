using ErabliereApi.Donnees.Ownable;
using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees;

/// <summary>
/// Une alerte d'un capteur
/// </summary>
public class AlerteCapteur : IIdentifiable<Guid?, AlerteCapteur>, ILevelTwoOwnable<Capteur>
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid? IdCapteur { get; set; }

    /// <summary>
    /// Le capteur de l'alerte
    /// </summary>
    public Capteur? Capteur { get; set; }

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
    /// Date création
    /// </summary>
    public DateTime? DC { get; set; }

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

    /// <inheritdoc />
    public Capteur? Owner { get => Capteur; set { Capteur = value; } }

    /// <inheritdoc />
    public Guid? OwnerId { get => IdCapteur; set { IdCapteur = value; } }

    /// <inheritdoc />
    public int CompareTo(AlerteCapteur? other)
    {
        return Id.HasValue ? Id.Value.CompareTo(other?.Id) : -1;
    }
}
