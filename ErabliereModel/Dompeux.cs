using System;

namespace ErabliereApi.Donnees
{
    public class Dompeux : IIdentifiable<int?>
    {
        /// <summary>
        /// Id du dompeux
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date de l'occurence
        /// </summary>
        public DateTime? T { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTime? DD { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTime? DF { get; set; }

        public int? IdErabliere { get; set; }
    }
}
