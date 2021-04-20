using System;

namespace ErabliereApi.Donnees
{
    public class DonneeCapteur : IIdentifiable<Guid?, DonneeCapteur>
    {
        public Guid? Id { get; set; }

        public short? Valeur { get; set; }

        /// <summary>
        /// La date de création
        /// </summary>
        public DateTimeOffset? D { get; set; }

        public int? IdCapteur { get; set; }

        public Capteur? Capteur { get; set; }

        public int CompareTo(DonneeCapteur? other)
        {
            return other.Id.Value.CompareTo(other.Id.Value);
        }
    }
}
