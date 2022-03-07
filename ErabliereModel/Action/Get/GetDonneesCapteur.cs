using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Modèle représentant une donnée capturé par un capteur
    /// </summary>
    public class GetDonneesCapteur
    {
        /// <summary>
        /// L'id de la donnée
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// La valeur de la donnée
        /// </summary>
        public short? Valeur { get; set; }

        /// <summary>
        /// Text associé à la donnée
        /// </summary>
        [MaxLength(50)]
        public string? Text { get; set; }

        /// <summary>
        /// La date de création
        /// </summary>
        public DateTimeOffset? D { get; set; }
    }
}
