using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle pour l'action modifier une note
/// </summary>
public class PutNote
{
    /// <summary>
    /// L'id de la note si le client désire l'initialiser
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// L'id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// Le titre de la note
    /// </summary>
    [MaxLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// Date de la note
    /// </summary>
    public DateTimeOffset? NoteDate { get; set; }

    /// <summary>
    /// Date de rappel de la note
    /// </summary>
    public DateTimeOffset? ReminderDate { get; set; }

    /// <summary>
    /// Filtre de notification. Permet à la note d'afficher comme une notification
    /// si le filtre est évaluer à vrai.
    /// </summary>
    [MaxLength(100)]
    public string? NotificationFilter { get; set; }

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
}
