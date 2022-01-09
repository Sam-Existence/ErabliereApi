using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees;

/// <summary>
/// Un baril
/// </summary>
public class Baril : IIdentifiable<Guid?, Baril>
{
    /// <summary>
    /// Id du baril
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Date ou le baril a été fermé
    /// </summary>
    public DateTimeOffset? DF { get; set; }

    /// <summary>
    /// L'id de l'érablière possédant le baril
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière possédant le baril
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Estimation de la qualité du sirop
    /// </summary>
    [MaxLength(15)]
    public string? QE { get; set; }

    /// <summary>
    /// Qualité du sirop après classement
    /// </summary>
    [MaxLength(15)]
    public string? Q { get; set; }

    /// <inheritdoc />
    public int CompareTo([AllowNull] Baril other)
    {
        if (other == default)
        {
            return 1;
        }

        if (DF.HasValue == false)
        {
            return other.DF.HasValue ? -1 : 0;
        }

        if (!other.DF.HasValue)
        {
            return -1;
        }

        return DF.Value.CompareTo(other.DF.Value);
    }
}
