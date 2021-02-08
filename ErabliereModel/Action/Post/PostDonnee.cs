using System;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostDonnee
    {
        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTime? D { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public short? T { get; set; }

        /// <summary>
        /// Niveau bassin
        /// </summary>
        public short? NB { get; set; }

        /// <summary>
        /// Vaccium
        /// </summary>
        public short? V { get; set; }

        /// <summary>
        /// Id de dl'érablière relier a cette donnée
        /// </summary>
        public int? IdErabliere { get; set; }
    }
}
