using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees.Action.Get
{
    public class GetBaril
    {
        /// <summary>
        /// Id du baril
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date ou le baril a été fermé
        /// </summary>
        public DateTimeOffset? DF { get; set; }

        /// <summary>
        /// L'id de l'érablière possédant le baril
        /// </summary>
        public int? IdErabliere { get; set; }

        /// <summary>
        /// Estimation de la qualité du sirop
        /// </summary>
        [MaxLength(15)]
        public string? QE { get; set; }

        /// <summary>
        /// Qualité du sirop après classement
        /// </summary>
        [MaxLength(15)]
        public string? Q { get; set; }
    }
}
