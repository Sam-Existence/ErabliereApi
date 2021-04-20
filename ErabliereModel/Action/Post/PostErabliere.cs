using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostErabliere
    {
        /// <summary>
        /// Le nom de l'érablière
        /// </summary>
        [MaxLength(50)]
        public string? Nom { get; set; }

        /// <summary>
        /// Spécifie les ip qui peuvent créer des opérations d'alimentation pour cette érablière.
        /// </summary>
        [MaxLength(50)]
        public string? IpRules { get; set; }

        /// <summary>
        /// Un indice permettant l'affichage des érablières dans l'ordre précisé.
        /// </summary>
        public int? IndiceOrdre { get; set; }

        /// <summary>
        /// Indicateur permettant de déterminer si la section des barils sera utiliser par l'érablière
        /// </summary>
        public bool? AfficherSectionBaril { get; set; }

        /// <summary>
        /// Indicateur permettant de déterminer si la section des donnees sera utiliser par l'érablière
        /// </summary>
        public bool? AfficherTrioDonnees { get; set; }

        /// <summary>
        /// Indicateur permettant de déterminer si la section des donnees sera utiliser par l'érablière
        /// </summary>
        public bool? AfficherSectionDompeux { get; set; }
    }
}
