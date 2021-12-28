using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Une alerte
    /// </summary>
    public class Alerte : IIdentifiable<Guid?, Alerte>
    {
        public Guid? Id { get; set; }
        public Guid? IdErabliere { get; set; }

        /// <summary>
        /// Une liste d'adresse email séparer par des ';'
        /// </summary>
        /// <example>exemple@courriel.com;exemple2@courriel.com</example>
        [MaxLength(200)]
        public string? EnvoyerA { get; set; }

        /// <summary>
        /// Si une temperature est reçu et que celle-ci est plus grande que cette valeur, cette validation sera évaluer à vrai.
        /// </summary>
        /// <example>0</example>
        [MaxLength(50)]
        public string? TemperatureThresholdLow { get; set; }

        [MaxLength(50)]
        public string? TemperatureThresholdHight { get; set; }

        [MaxLength(50)]
        public string? VacciumThresholdLow { get; set; }

        /// <summary>
        /// Si un vaccium est reçu et que celui-ci est plus petit que cette valeur, cette validation sera évaluer à vrai.
        /// </summary>
        /// <example>200</example>
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
