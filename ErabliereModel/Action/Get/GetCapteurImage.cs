using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Get
{
    /// <summary>
    /// Modèle de retour de l'action d'obtention d'un capteur
    /// </summary>
    public class GetCapteurImage
    {
        /// <summary>
        /// Le nom donné au capteur
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// L'url du capteur (au protocol rtsp)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Le port du capteur.
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// L'identifiant utilisé pour se connecter au flux d'image.
        /// </summary>
        public string? Identifiant { get; set; }

        /// <summary>
        /// Le mot de passe utilisé pour se connecter au flux d'image.
        /// </summary>
        public string? MotDePasse { get; set; }

        /// <summary>
        /// L'ordre auquel le capteur doit être affiché.
        /// </summary>
        public int Ordre { get; set; }
    }
}

