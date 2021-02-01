using System;

namespace ErabliereApi.Donnees
{
    public class Donnee : IIdentifiable<int?>
    {
        /// <summary>
        /// L'id de l'occurence
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTime? D { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public short? T { get; set; }

        /// <summary>
        /// Niveau bassin
        /// </summary>
        public short? NB { get; set; }

        /// <summary>
        /// Vaccium
        /// </summary>
        public short? V { get; set; }
    }
}
