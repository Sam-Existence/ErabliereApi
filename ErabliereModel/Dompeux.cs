using System;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees;

/// <summary>
/// Représente un dompeux et certaine informations relié
/// </summary>
public class Dompeux : IIdentifiable<Guid?, Dompeux>
{
    /// <summary>
    /// Id du dompeux
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Date de l'occurence
    /// </summary>
    public DateTimeOffset? T { get; set; }

    /// <summary>
    /// La date de début
    /// </summary>
    public DateTimeOffset? DD { get; set; }

    /// <summary>
    /// La date de début
    /// </summary>
    public DateTimeOffset? DF { get; set; }

    /// <summary>
    /// Clé étrangère de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière relié
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <inheritdoc />
    public int CompareTo([AllowNull] Dompeux other)
    {
        if (other == default)
        {
            return 1;
        }

        if (T.HasValue == false)
        {
            return other.T.HasValue ? -1 : 0;
        }

        if (!other.T.HasValue)
        {
            return -1;
        }

        return T.Value.CompareTo(other.T.Value);
    }
}
