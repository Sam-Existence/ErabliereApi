using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostRappel
    {
        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public Guid? IdErabliere { get; set; }

        /// <summary>
        /// Indique si le rappel est actif
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// La date du rappel
        /// </summary>
        public DateTimeOffset? DateRappel { get; set; }

        /// <summary>
        /// La périodicité du rappel
        /// </summary>
        [MaxLength(20, ErrorMessage = "La périodicité du rappel ne peut pas dépasser 20 caractères.")]
        public string? Periodicite { get; set; }
    }
}
