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
    [MaxLength(200, ErrorMessage = "Le titre de la note ne peut pas dépasser 200 caractères.")]
    public string? Title { get; set; }

    /// <summary>
    /// Le text de la note
    /// </summary>
    [MaxLength(2000, ErrorMessage = "Le text de la note ne peut pas dépasser 2000 caractères.")]
    public string? Text { get; set; }

    /// <summary>
    /// L'extension du fichier
    /// </summary>
    [MaxLength(20, ErrorMessage = "L'extension du fichier ne peut pas dépasser 20 caractères.")]
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
    [MaxLength(100, ErrorMessage = "Le filtre de notification ne peut pas dépasser 100 caractères.")]
    public string? NotificationFilter { get; set; }

    /// <summary>
    /// Date de rappel de la note
    /// </summary>
    public DateTimeOffset? ReminderDate { get; set; }

    /// <summary>
    /// Le rappel associé à la note
    /// </summary>
    public Rappel? Rappel { get; set; }

    /// <inheritdoc />
    public int CompareTo(Note? other)
    {
        return 0;
    }
}
