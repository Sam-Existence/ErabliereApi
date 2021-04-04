using System;
using System.ComponentModel.DataAnnotations;
namespace ErabliereApi.Donnees
{
    public class Capteur : IIdentifiable<int?, Capteur>
    {
        /// <summary>
        /// L'id de l'occurence
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Date de l'ajout du capteur
        /// </summary>
        public DateTimeOffset? DC { get; set; }

        /// <summary>
        /// Id de dl'érablière relier a cette donnée
        /// </summary>
        public int? IdErabliere { get; set; }

        /// <summary>
        /// Le nom donné au capteur
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Nom { get; set; }

        public int CompareTo(Capteur? other)
        {
            return Nom?.CompareTo(other?.Nom) ?? 1;
        }
    }
}
