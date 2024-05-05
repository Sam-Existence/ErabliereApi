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
        /// Position dans la liste
        /// </summary>
        public int? Position { get; set; }
    }
}