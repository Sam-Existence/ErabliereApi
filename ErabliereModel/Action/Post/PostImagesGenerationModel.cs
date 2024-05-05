namespace ErabliereApi.Donnees.Action.Post;

/// <summary>
/// Modèle pour la génération d'images
/// </summary>
public class PostImagesGenerationModel
{
    /// <summary>
    /// Nombre d'images à générer
    /// </summary>
    public int? ImageCount { get; set; }

    /// <summary>
    /// La requête pour la génération d'image
    /// </summary>
    public string Prompt { get; set; } = "";

    /// <summary>
    /// Taille des images
    /// </summary>
    /// <example>100x100</example>
    public string? Size { get; set; }
}
