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

        /// <inheritdoc />
        public int CompareTo([AllowNull] Erabliere other)
        {
            if (IndiceOrdre != null && other?.IndiceOrdre == null)
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