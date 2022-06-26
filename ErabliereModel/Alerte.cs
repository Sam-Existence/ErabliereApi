using ErabliereApi.Donnees.Ownable;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees;

/// <summary>
/// Une alerte
/// </summary>
public class Alerte : IIdentifiable<Guid?, Alerte>, IErabliereOwnable
{
    /// <summary>
    /// L'id de l'alerte
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// L'id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <inheritdoc />
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Une liste d'adresse email séparer par des ';'
    /// </summary>
    /// <example>exemple@courriel.com;exemple2@courriel.com</example>
    [MaxLength(200)]
    public string? EnvoyerA { get; set; }

    /// <summary>
    /// Si une temperature est reçu et que celle-ci est plus grande que cette valeur, cette validation sera évaluer à vrai.
    /// </summary>
    /// <example>0</example>
    [MaxLength(50)]
    public string? TemperatureThresholdLow { get; set; }

    /// <summary>
    /// Pourrait être interprété comme TemperatureMinValue
    /// </summary>
    [MaxLength(50)]
    public string? TemperatureThresholdHight { get; set; }

    /// <summary>
    /// Pourrait être interprété comme VacciumMaxValue
    /// </summary>
    [MaxLength(50)]
    public string? VacciumThresholdLow { get; set; }

    /// <summary>
    /// Si un vaccium est reçu et que celui-ci est plus petit que cette valeur, cette validation sera évaluer à vrai.
    /// </summary>
    /// <example>200</example>
    [MaxLength(50)]
    public string? VacciumThresholdHight { get; set; }

    /// <summary>
    /// Pourrait être interprété comme NiveauBassinMaxValue
    /// </summary>
    [MaxLength(50)]
    public string? NiveauBassinThresholdLow { get; set; }

    /// <summary>
    /// Pourrait être interprété comme NiveauBassinMinValue
    /// </summary>
    [MaxLength(50)]
    public string? NiveauBassinThresholdHight { get; set; }

    /// <summary>
    /// Indique si l'alerte est activé
    /// </summary>
    public bool IsEnable { get; set; }

    /// <inheritdoc />
    public int CompareTo([AllowNull] Alerte other)
    {
        if (other == null)
        {
            return 1;
        }

        if (Id == null)
        {
            return other.Id == null ? 0 : -1;
        }

        if (Id != null && other.Id == null)
        {
            return 1;
        }

        if (Id == null)
        {
            return -1;
        }

        return Id.Value.CompareTo(other.Id);
    }
}
