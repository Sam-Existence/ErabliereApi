using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle de création d'un capteur
    /// </summary>
    public class PostCapteur
    {
        /// <summary>
        /// L'id guid si le client désire initialiser l'id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Le nom du capteur
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Nom { get; set; }

        /// <summary>
        /// Le symbole utilisé pour l'affichage des valeurs
        /// </summary>
        [MaxLength(5)]
        public string? Symbole { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }

        /// <summary>
        /// Indicateur permettant d'afficher ou non le graphique relié au capteur.
        /// </summary>
        public bool? AfficherCapteurDashboard { get; set; }

        /// <summary>
        /// Indicateur peremettant d'afficher un formulaire quand les données
        /// peuvent être saisit depuis un interface utilisateur.
        /// </summary>
        public bool? AjouterDonneeDepuisInterface { get; set; }

        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public Guid? IdErabliere { get; set; }
    }
}
