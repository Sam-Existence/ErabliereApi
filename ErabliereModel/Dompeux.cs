using System;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Dompeux : IIdentifiable<int?, Dompeux>
    {
        /// <summary>
        /// Id du dompeux
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date de l'occurence
        /// </summary>
        public DateTimeOffset? T { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTimeOffset? DD { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTimeOffset? DF { get; set; }

        public int? IdErabliere { get; set; }

        public Erabliere? Erabliere { get; set; }

        public int CompareTo([AllowNull] Dompeux other)
        {
            if (other == default)
            {
                return 1;
            }

            if (T.HasValue == false)
            {
                return other.T.HasValue ? -1 : 0;
            }

            return T.Value.CompareTo(other.T.Value);
        }
    }
}
