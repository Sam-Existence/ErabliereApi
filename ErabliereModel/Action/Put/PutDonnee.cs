using System;
using System.Collections.Generic;
using System.Text;

namespace ErabliereApi.Donnees.Action.Put
{
    public class PutDonnee
    {
        /// <summary>
        /// Id de la donnée à modifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Vaccium en dixième de HG
        /// </summary>
        public short? V { get; set; }

        /// <summary>
        /// Id de dl'érablière relier a cette donnée
        /// </summary>
        public int? IdErabliere { get; set; }
    }
}
