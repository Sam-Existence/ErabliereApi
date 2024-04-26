using ErabliereApi.Donnees.Ownable;
using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Un rappel
    /// </summary>
    public class Rappel : IIdentifiable<Guid?, Rappel>, IErabliereOwnable
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
        /// L'érablière qui possède le rappel
        /// </summary>
        public Erabliere? Erabliere { get; set; }

        /// <summary>
        /// La date du rappel
        /// </summary>
        public DateTimeOffset? DateRappel { get; set; }

        /// <summary>
        /// La périodicité du rappel
        /// </summary>
        [MaxLength(20, ErrorMessage = "La périodicité du rappel ne peut pas dépasser 20 caractères.")]
        public string? Periodicite { get; set; } // Annuel, Mensuel, Hebdo, Quotidien

        /// <summary>
        /// La clé étrangère de la note
        /// </summary>
        public Guid? NoteId { get; set; }

        /// <summary>
        /// La note qui possède le rappel
        /// </summary>
        public Note? Note { get; set; }

        /// <inheritdoc />
        public int CompareTo(Rappel? other)
        {
            return 0;
        }
    }
}
