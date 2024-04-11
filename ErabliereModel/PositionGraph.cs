using System;
using System.Diagnostics.CodeAnalysis;
using ErabliereApi.Donnees.Ownable;

namespace ErabliereApi.Donnees;

/// <summary>
/// Modèle position graph.
/// </summary>
public class PositionGraph : IIdentifiable<Guid?, PositionGraph>, IErabliereOwnable
{
    /// <summary>
    /// L'id de l'occurence
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Date de la transaction
    /// </summary>
    public DateTimeOffset? D { get; set; }

    /// <summary>
    /// Position sur l'axe des x
    /// </summary>
    public short? PX { get; set; }

    /// <summary>
    /// Position sur l'axe des y
    /// </summary>
    public short? PY { get; set; }

    /// <summary>
    /// Id de l'érablière relié a cette donnée
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'erabliere relié à la donn�e
    /// </summary>
    public Erabliere? Erabliere { get; set; }

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