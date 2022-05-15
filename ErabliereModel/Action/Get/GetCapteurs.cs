using System;

namespace ErabliereApi.Donnees.Action.Get
{
    /// <summary>
    /// Modèle de retour de l'action d'obtention d'un capteur
    /// </summary>
    public class GetCapteurs
    {
        /// <summary>
        /// L'id du catpeur
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Le nom du capteur
        /// </summary>
        public string? Nom { get; set; }

        /// <summary>
        /// Le symbole du capteur
        /// </summary>
        public string? Symbole { get; set; }

        /// <summary>
        /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
        /// </summary>
        public bool? AfficherCapteurDashboard { get; set; }

        /// <summary>
        /// Indicateur si les données sont ajouter depuis un interface graphique
        /// </summary>
        public bool AjouterDonneeDepuisInterface { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }
    }
}
