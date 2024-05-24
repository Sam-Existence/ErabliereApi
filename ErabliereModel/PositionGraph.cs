using System;
using System.Diagnostics.CodeAnalysis;
using ErabliereApi.Donnees.Ownable;

namespace ErabliereApi.Donnees;

/// <summary>
/// Modèle position graph.
/// </summary>
public class PositionGraph : IIdentifiable<Guid?, PositionGraph>
{
    /// <summary>
    /// L'id du capteur
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Id de l'érablière relié a cette donnée
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// Date de la transaction
    /// </summary>
    public DateTimeOffset? D { get; set; }

    /// <summary>
    /// Position dans la liste
    /// </summary>
    public int? Position { get; set; }

    /// <inheritdoc />
    public int CompareTo([AllowNull] PositionGraph other)
    {
        if (other == default)
        {
            return 1;
        }

        if (D.HasValue == false)
        {
            return other.D.HasValue ? -1 : 0;
        }

        if (!other.D.HasValue)
        {
            return 1;
        }

        return D.Value.CompareTo(other.D.Value);
    }
}