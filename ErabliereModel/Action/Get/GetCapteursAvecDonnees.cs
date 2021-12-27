using System;
using System.Collections.Generic;

namespace ErabliereApi.Donnees.Action.Get
{
    public class GetCapteursAvecDonnees
    {
        public Guid? Id { get; set; }

        public string? Nom { get; set; }

        public Guid? IdErabliere { get; set; }

        /// <summary>
        /// La date de création de l'entité.
        /// </summary>
        public DateTimeOffset? DC { get; set; }

        public List<GetDonneesCapteur>? Donnees { get; set; }
    }
}
