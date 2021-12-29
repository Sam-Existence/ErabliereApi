using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees
{
    public class AlerteCapteur : IIdentifiable<Guid?, AlerteCapteur>
    {
        /// <summary>
        /// La clé primaire
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// L'id du capteur
        /// </summary>
        public Guid? IdCapteur { get; set; }

        /// <summary>
        /// Le capteur de l'alerte
        /// </summary>
        public Capteur? Capteur { get; set; }

        /// <summary>
        /// Une liste d'adresse email séparer par des ';'
        /// </summary>
        /// <example>exemple@courriel.com;exemple2@courriel.com</example>
        [MaxLength(200)]
        public string? EnvoyerA { get; set; }

        /// <summary>
        /// Date création
        /// </summary>
        public DateTime? DC { get; set; }

        /// <summary>
        /// La valeur minimal de ce capteur
        /// </summary>
        public short? MinVaue { get; set; }

        /// <summary>
        /// La valeur maximal de ce capteur
        /// </summary>
        public short? MaxValue { get; set; }

        /// <inheritdoc />
        public int CompareTo(AlerteCapteur? other)
        {
            return Id.HasValue ? Id.Value.CompareTo(other?.Id) : -1;
        }
    }
}
