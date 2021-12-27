using System;

namespace ErabliereApi.Donnees
{
    public class DonneeCapteur : IIdentifiable<Guid?, DonneeCapteur>
    {
        /// <summary>
        /// L'id de la donnée du capteur
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// La valeur de la donnée
        /// </summary>
        public short? Valeur { get; set; }

        /// <summary>
        /// La date de création
        /// </summary>
        public DateTimeOffset? D { get; set; }

        /// <summary>
        /// L'id du capteur de la donnée
        /// </summary>
        public Guid? IdCapteur { get; set; }

        public Capteur? Capteur { get; set; }

        public int CompareTo(DonneeCapteur? other)
        {
            return other.Id.Value.CompareTo(other.Id.Value);
        }
    }
}
