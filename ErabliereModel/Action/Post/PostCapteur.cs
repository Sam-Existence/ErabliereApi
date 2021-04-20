using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostCapteur
    {
        public string? Nom { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }

        /// <summary>
        /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
        /// </summary>
        public bool? AfficherCapteurDashboard { get; set; }

        public int? IdErabliere { get; set; }
    }
}
