using ErabliereApi.Donnees.Ownable;
using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees;

/// <summary>
/// Une note
/// </summary>
public class Note : IIdentifiable<Guid?, Note>, IErabliereOwnable
{
    /// <summary>
    /// La clé primaire
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// La clé étrangère de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// L'érablière qui possède la note
    /// </summary>
    public Erabliere? Erabliere { get; set; }

    /// <summary>
    /// Le titre de la note
    /// </summary>
    [MaxLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// Le text de la note
    /// </summary>
    [MaxLength(2000)]
    public string? Text { get; set; }

    /// <summary>
    /// L'extension du fichier
    /// </summary>
    [MaxLength(20)]
    public string? FileExtension { get; set; }

    /// <summary>
    /// La fichier de la note
    /// </summary>
    public byte[]? File { get; set; }

    /// <summary>
    /// Date de cération de la note
    /// </summary>
    public DateTimeOffset? Created { get; set; }

    /// <summary>
    /// Date de la note
    /// </summary>
    public DateTimeOffset? NoteDate { get; set; }

    /// <summary>
    /// Filtre de notification. Permet à la note d'afficher comme une notification
    /// si le filtre est évaluer à vrai.
    /// </summary>
    [MaxLength(100)]
    public string? NotificationFilter { get; set; }

    /// <inheritdoc />
    public int CompareTo(Note? other)
    {
        return 0;
    }
}
