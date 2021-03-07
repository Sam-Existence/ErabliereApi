using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ErabliereApi.Donnees.Action.Put
{
    public class PutErabliere
    {
        /// <summary>
        /// L'id de l'érablière à modifier.
        /// </summary>
        [Required]
        public int? Id { get; set; }

        /// <summary>
        /// Le nouveau nom de l'érablière, si le nom est modifié
        /// </summary>
        [MaxLength(50)]
        public string? Nom { get; set; }

        /// <summary>
        /// Spécifie les ip qui peuvent créer des opérations d'alimentation pour cette érablière. Doivent être séparé par des ';'
        /// </summary>
        [MaxLength(50)]
        public string? IpRule { get; set; }

        /// <summary>
        /// Un indice permettant l'affichage des érablières dans l'ordre précisé.
        /// </summary>
        public int? IndiceOrdre { get; set; }
    }
}
