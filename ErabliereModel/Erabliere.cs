using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Erabliere : IIdentifiable<int?, Erabliere>
    {
        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Le nom de l'érablière
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Nom { get; set; }

        /// <summary>
        /// Addresse IP alloué à faire des opération d'écriture
        /// </summary>
        [MaxLength(50)]
        public string? IpRule { get; set; }

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

        /// <summary>
        /// Les capteurs de l'érablière
        /// </summary>
        public List<Capteur>? Capteurs { get; set; }

        /// <summary>
        /// Les données relier à l'érablière
        /// </summary>
        public List<Donnee>? Donnees { get; set; }

        /// <summary>
        /// La liste des barils de l'érablière
        /// </summary>
        public List<Baril>? Barils { get; set; }

        /// <summary>
        /// La liste des dompeux de l'érablière
        /// </summary>
        public List<Dompeux>? Dompeux { get; set; }

        /// <inheritdoc />
        public int CompareTo([AllowNull] Erabliere other)
        {
            if (IndiceOrdre.HasValue && other?.IndiceOrdre == null)
            {
                return -1;
            }
            else if (other?.IndiceOrdre.HasValue == true && IndiceOrdre.HasValue == false)
            {
                return 1;
            }
            else if (IndiceOrdre.HasValue && other?.IndiceOrdre.HasValue == true)
            {
                return IndiceOrdre.Value.CompareTo(IndiceOrdre);
            }

            return string.Compare(Nom, other?.Nom);
        }
    }
}