using System;

namespace ErabliereApi.Donnees.Action.Get
{
    /// <summary>
    /// Modèle de retour d'obtention de la position
    /// </summary>
    public class GetPositonGraph
    {
        /// <summary>
        /// L'id de l'occurence
        /// </summary>
        public int? Id { get; set; }

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
    }
}