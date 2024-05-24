using System;
using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    /// <summary>
    /// Modèle de création d'une entité <see cref="PositionGraph"/>
    /// </summary>
    public class PostPositionGraph
    {
        ///<summary>
        /// L'id de l'occurence
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTimeOffset? D { get; set; }

        /// <summary>
        /// Position dans la liste
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Id de l'érablière relier a cette donnée
        /// </summary>
        [Required]
        public Guid? IdErabliere { get; set; }
    }
}