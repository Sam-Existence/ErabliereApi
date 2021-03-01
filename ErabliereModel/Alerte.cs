using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Alerte : IIdentifiable<int?, Alerte>
    {
        public int? Id { get; set; }
        public int? IdErabliere { get; set; }

        /// <summary>
        /// Une liste d'adresse email séparer par des ';'
        /// </summary>
        [MaxLength(50)]
        public string? EnvoyerA { get; set; }

        [MaxLength(50)]
        public string? TemperatureThresholdLow { get; set; }

        [MaxLength(50)]
        public string? TemperatureThresholdHight { get; set; }

        [MaxLength(50)]
        public string? VacciumThresholdLow { get; set; }

        [MaxLength(50)]
        public string? VacciumThresholdHight { get; set; }

        [MaxLength(50)]
        public string? NiveauBassinThresholdLow { get; set; }

        [MaxLength(50)]
        public string? NiveauBassinThresholdHight { get; set; }

        /// <inheritdoc />
        public int CompareTo([AllowNull] Alerte other)
        {
            if (other == null)
            {
                return 1;
            }

            if (Id == null)
            {
                return other.Id == null ? 0 : -1;
            }

            if (Id != null && other.Id == null)
            {
                return 1;
            }

            return Id.Value.CompareTo(other.Id);
        }
    }
}
