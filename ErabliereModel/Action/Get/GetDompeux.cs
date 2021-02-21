using System;
using System.Collections.Generic;
using System.Text;

namespace ErabliereApi.Donnees.Action.Get
{
    public class GetDompeux
    {
        /// <summary>
        /// Id du dompeux
        /// </summary>
        public int? Id { get; set; }

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
