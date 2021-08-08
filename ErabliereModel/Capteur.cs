using System;
using System.Collections.Generic;
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
        /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
        /// </summary>
        public bool? AfficherCapteurDashboard { get; set; }

        /// <summary>
        /// Id de dl'érablière relier a cette donnée
        /// </summary>
        public int? IdErabliere { get; set; }

        /// <summary>
        /// L'érablière de ce capteur
        /// </summary>
        public Erabliere? Erabliere { get; set; }

        /// <summary>
        /// Les données du capteurs
        /// </summary>
        public List<DonneeCapteur>? DonneesCapteur { get; set; }

        /// <summary>
        /// Le nom donné au capteur
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Nom { get; set; }

        /// <summary>
        /// Le symbole qui représente l'unité observer par le capteur.
        /// <summary>
        /// <example>
        /// "°c" pour représenter la temperature en celcius
        /// </example>
        [Required]
        [MaxLength(5)]
        public string? Symbole { get; set; }

        public int CompareTo(Capteur? other)
        {
            return Nom?.CompareTo(other?.Nom) ?? 1;
        }
    }
}
