using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Put
{
    /// <summary>
    /// Modèle de modification d'une entité <see cref="PositionGraph"/>
    /// </summary>
    public class PutPositionGraph
    {
        ///<summary>
        /// L'id de l'occurence
        /// </summary>
        public int? Id { get; set; }

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
