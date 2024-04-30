using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Put
{
    public class PutRappel
    {
        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public Guid IdErabliere { get; set; }

        /// <summary>
        /// Indique si le rappel est actif
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// La date du rappel
        /// </summary>
        public DateTimeOffset? DateRappel { get; set; }

        /// <summary>
        /// La periodicité du rappel
        /// </summary>
        [MaxLength(20, ErrorMessage = "The periodicity of the reminder cannot exceed 20 characters.")]
        public string? Periodicite { get; set; }
    }
}
