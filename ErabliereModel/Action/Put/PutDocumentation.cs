using System;

namespace ErabliereApi.Donnees.Action.Put;

/// <summary>
/// Modèle pour la modification d'une documentation
/// </summary>
public class PutDocumentation
{
    /// <summary>
    /// Id de la documentation
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Id de l'érablière
    /// </summary>
    public Guid? IdErabliere { get; set; }

    /// <summary>
    /// Titre de la documentation
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Text de la documentation
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Extension du fichier de la documentation
    /// </summary>
    public string? FileExtension { get; set; }
}
