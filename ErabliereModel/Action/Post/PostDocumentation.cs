using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle d'ajout de documentation
    /// </summary>
    public class PostDocumentation
    {
        /// <summary>
        /// L'id si le client désire l'initialiser
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// L'id de l'érablière relié
        /// </summary>
        [Required]
        public Guid? IdErabliere { get; set; }

        /// <summary>
        /// Date de création de la documentation
        /// </summary>
        public DateTimeOffset? Created { get; set; }

        /// <summary>
        /// Le titre de la documentation
        /// </summary>
        [MaxLength(200)]
        public string? Title { get; set; }

        /// <summary>
        /// Text de la documentation
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// L'extension du fichier
        /// </summary>
        [MaxLength(20)]
        public string? FileExtension { get; set; }

        /// <summary>
        /// Base64 string
        /// </summary>
        public string? File { get; set; }
    }
}
