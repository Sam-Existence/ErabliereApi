using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle d'ajout d'une note
    /// </summary>
    public class PostNote
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
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// Le text de la note
        /// </summary>
        [MaxLength(2000)]
        public string? Text { get; set; }

        /// <summary>
        /// Base64 string
        /// </summary>
        public string? File { get; set; }

        /// <summary>
        /// L'extension du fichier
        /// </summary>
        [MaxLength(20)]
        public string? FileExtension { get; set; }

        /// <summary>
        /// La date de cération
        /// </summary>
        public DateTimeOffset? Created { get; set; }

        /// <summary>
        /// La date de la note
        /// </summary>
        public DateTimeOffset? NoteDate { get; set; }
    }
}
