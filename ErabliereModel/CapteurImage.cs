using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees
{
    public class CapteurImage
    {
        /// <summary>
        /// L'id du capteur
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id de l'érablière lié à ce capteur
        /// </summary>
        public Guid IdErabliere { get; set; }

        /// <summary>
        /// L'érablière liée à ce capteur
        /// </summary>
        public Erabliere? Erabliere { get; set; }


        /// <summary>
        /// Le nom donné au capteur
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Nom { get; set; }

        /// <summary>
        /// L'url du capteur (au protocol rtsp)
        /// </summary>
        /// <example>
        /// rtsp://url-de-votre-capteur.com
        /// </example>
        [Required]
        [MaxLength(200)]
        [RegularExpression("/^rtsp:\\/\\/[-a-zA-Z0-9@:%._+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_+.~#?&/=]*)$/i")]
        public string Url { get; set; }

        /// <summary>
        /// Le port du capteur.
        /// </summary>
        [Required]
        [MaxLength(5)]
        public string Port { get; set; }

        /// <summary>
        /// L'identifiant utilisé pour se connecter au flux d'image.
        /// </summary>
        [MaxLength(200)]
        public string? Identifiant { get; set; }

        /// <summary>
        /// Le mot de passe utilisé pour se connecter au flux d'image.
        /// </summary>
        [MaxLength(200)]
        public string? MotDePasse { get; set; }
    }
}
