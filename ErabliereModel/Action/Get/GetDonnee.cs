using System;
using System.Collections.Generic;
using System.Text;

namespace ErabliereApi.Donnees.Action.Get
{
    public class GetDonnee
    {
        /// <summary>
        /// L'id de l'occurence
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Date de la transaction
        /// </summary>
        public DateTimeOffset? D { get; set; }

        /// <summary>
        /// Temperature en dixième de celcius
        /// </summary>
        public short? T { get; set; }

        /// <summary>
        /// Niveau bassin en pourcentage
        /// </summary>
        public short? NB { get; set; }

        /// <summary>
        /// Vaccium en dixième de HG
        /// </summary>
        public short? V { get; set; }

        /// <summary>
        /// Interval de date des données alimenté. Utiliser pour optimiser le nombre de données enregistrer
        /// 
        /// Plus grand interval d'alimentation de cette donnée, en seconde
        /// </summary>
        public int? PI { get; set; }

        /// <summary>
        /// Nombre d'occurence enrgistrer de cette donnée
        /// </summary>
        public int Nboc { get; set; }

        /// <summary>
        /// Id donnée précédente
        /// </summary>
        public Guid? Iddp { get; set; }
    }
}
