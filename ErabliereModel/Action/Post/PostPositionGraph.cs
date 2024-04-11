using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle de création d'une entité <see cref="PositionGraph"/>
    /// </summary>
    public class PostPositionGraph
    {
        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTimeOffset? D { get; set; }

        /// <summary>
        /// Position sur l'axe des x
        /// </summary>
        public short? PX { get; set; }

        /// <summary>
        /// Position sur l'axe des y
        /// </summary>
        public short? PY { get; set; }

        /// <summary>
        /// Id de l'érablière relier a cette donnée
        /// </summary>
        [Required]
        public Guid? IdErabliere { get; set; }
    }
}