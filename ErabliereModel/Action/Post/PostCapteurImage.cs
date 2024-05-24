using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle de création d'un capteur d'image
    /// </summary>
    public class PostCapteurImage
    {
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
        [RegularExpression(@"^rtsp:\/\/[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}([-a-zA-Z0-9()@:%_+.~#?&/=]*)$")]
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
