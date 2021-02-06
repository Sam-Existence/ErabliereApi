using System;

namespace ErabliereApi.Donnees
{
    public class Baril : IIdentifiable<int?>
    {
        /// <summary>
        /// Id du baril
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date ou le baril a été fermé
        /// </summary>
        public DateTime? DF { get; set; }

        public int? IdErabliere { get; set; }

        /// <summary>
        /// Estimation de la qualité du sirop
        /// </summary>
        public string? QE { get; set; }

        /// <summary>
        /// Qualité du sirop après classement
        /// </summary>
        public string? Q { get; set; }
    }
}
