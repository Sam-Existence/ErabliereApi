using System;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Une données d'un capteur
    /// </summary>
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

        /// <summary>
        /// Le capteur de la donnée
        /// </summary>
        public Capteur? Capteur { get; set; }

        /// <inheritdoc />
        public int CompareTo(DonneeCapteur? other)
        {
            if (other == default)
            {
                return 1;
            }


            if (!other.Id.HasValue)
            {
                return -1;
            }

            return other.Id.Value.CompareTo(other.Id.Value);
        }
    }
}
