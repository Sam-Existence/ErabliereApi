using System;

namespace ErabliereApi.Donnees.Action.Get
{
    /// <summary>
    /// Modèle de retour de l'action d'obtention d'un capteur
    /// </summary>
    public class GetCapteur
    {
        /// <summary>
        /// L'id du catpeur
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public Guid? IdErabliere { get; set; }

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

        /// <summary>
        /// L'indice du tri
        /// </summary>
        public int? IndiceOrdre { get; set; }

        /// <summary>
        /// la string bootstrap pour chnager les dimensions du graphique
        /// </summary>
        public string? Taille { get; set; }
    }
}
