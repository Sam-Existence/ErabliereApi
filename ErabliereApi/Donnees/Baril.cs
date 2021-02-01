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
    }
}
