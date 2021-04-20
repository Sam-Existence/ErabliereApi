using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Action.Get
{
    public class GetCapteurs
    {
        public int? Id { get; set; }

        public string? Nom { get; set; }

        public int? IdErabliere { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }
    }
}
