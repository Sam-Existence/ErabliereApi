using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [MaxLength(200, ErrorMessage = "Le titre de la note ne peut pas dépasser 200 caractères.")]
        [Required(ErrorMessage = "Le titre de la note est requis.")]
        public string? Title { get; set; }

        /// <summary>
        /// Le text de la note
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Le texte de la note ne peut pas dépasser 2000 caractères.")]
        public string? Text { get; set; }

        /// <summary>
        /// Base64 string
        /// </summary>
        public string? File { get; set; }

        /// <summary>
        /// Les bytes du fichier.
        /// </summary>
        [JsonIgnore]
        public byte[]? FileBytes { get; set; }

        /// <summary>
        /// L'extension du fichier
        /// </summary>
        [MaxLength(20, ErrorMessage = "L'extension du fichier ne peut pas dépasser 20 caractères.")]
        public string? FileExtension { get; set; }

        /// <summary>
        /// La date de cération
        /// </summary>
        public DateTimeOffset? Created { get; set; }

        /// <summary>
        /// La date de la note
        /// </summary>
        public DateTimeOffset? NoteDate { get; set; }

        /// <summary>
        /// Rappel associé à la note
        /// </summary>
        public PostRappel? Rappel { get; set; }

        /// <summary>
        /// Validation du fichier en base64 avec stockage des bytes
        /// sur la propriété FileBytes
        /// </summary>
        public bool IsValidBase64()
        {
            if (File == null)
            {
                FileBytes = null;

                return false;
            }

            try
            {
                FileBytes = Convert.FromBase64String(File);

                return true;
            }
            catch
            {
                FileBytes = null;

                return false;
            }
        }
    }
}
