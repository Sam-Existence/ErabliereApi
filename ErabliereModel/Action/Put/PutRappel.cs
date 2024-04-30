using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Put
{
    public class PutRappel
    {
        /// <summary>
        /// The ID of the erabliere
        /// </summary>
        public Guid IdErabliere { get; set; }

        /// <summary>
        /// The date of the reminder
        /// </summary>
        public DateTimeOffset? DateRappel { get; set; }

        /// <summary>
        /// The periodicity of the reminder
        /// </summary>
        [MaxLength(20, ErrorMessage = "The periodicity of the reminder cannot exceed 20 characters.")]
        public string? Periodicite { get; set; }
    }
}
