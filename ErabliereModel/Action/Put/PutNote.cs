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
    /// L'extension du fichier
    /// </summary>
    [MaxLength(20)]
    public string? FileExtension { get; set; }
}
