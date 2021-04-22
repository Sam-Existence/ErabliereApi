using System;

namespace ErabliereApi.Donnees.Action.Put
{
    public class PutCapteur
    {
        public int? Id { get; set; }

        public string? Nom { get; set; }

        public int? IdErabliere { get; set; }

        /// <summary>
        /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
        /// </summary>
        public bool? AfficherCapteurDashboard { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }
    }
}