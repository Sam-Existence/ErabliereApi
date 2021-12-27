using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle de création d'une entité <see cref="Donnee"/>
    /// </summary>
    public class PostDonnee
    {
        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTimeOffset? D { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public short? T { get; set; }

        /// <summary>
        /// Niveau bassin
        /// </summary>
        public short? NB { get; set; }

        /// <summary>
        /// Vaccium
        /// </summary>
        public short? V { get; set; }

        /// <summary>
        /// Id de dl'érablière relier a cette donnée
        /// </summary>
        [Required]
        public Guid? IdErabliere { get; set; }
    }
}
