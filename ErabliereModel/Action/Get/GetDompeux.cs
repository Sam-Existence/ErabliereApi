using System;

namespace ErabliereApi.Donnees.Action.Get
{
    /// <summary>
    /// Modèle de retour d'obtention d'un ou plusieurs dompeux
    /// </summary>
    public class GetDompeux
    {
        /// <summary>
        /// Id du dompeux
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Date de l'occurence
        /// </summary>
        public DateTimeOffset? T { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTimeOffset? DD { get; set; }

        /// <summary>
        /// La date de début
        /// </summary>
        public DateTimeOffset? DF { get; set; }
    }
}
