using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees;

/// <summary>
/// Une documentation
/// </summary>
public class Documentation : IIdentifiable<Guid?, Documentation>
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Clé étrangère de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière relié
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Date d'ajout de la docuemntation
    /// </summary>
    public DateTimeOffset? Created { get; set; }

    /// <summary>
    /// Le titre de la documentation
    /// </summary>
    [MaxLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// Le text de la documentation
    /// </summary>
    [MaxLength(2000)]
    public string? Text { get; set; }

    /// <summary>
    /// L'extension du fichier
    /// </summary>
    [MaxLength(20)]
    public string? FileExtension { get; set; }

    /// <summary>
    /// Le fichier de la documentation
    /// </summary>
    public byte[]? File { get; set; }

    /// <inheritdoc />
    public int CompareTo(Documentation? other)
    {
        return 0;
    }
}
