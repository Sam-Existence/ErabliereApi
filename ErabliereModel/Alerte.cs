using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Alerte : IIdentifiable<int?, Alerte>
    {
        public int? Id { get; set; }
        public int? IdErabliere { get; set; }
        public List<string> EnvoyerA { get; set; } = new List<string>();
        public string? TemperatureThresholdLow { get; set; }
        public string? TemperatureThresholdHight { get; set; }
        public string? VacciumThresholdLow { get; set; }
        public string? VacciumThresholdHight { get; set; }
        public string? NiveauBassinThresholdLow { get; set; }
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
