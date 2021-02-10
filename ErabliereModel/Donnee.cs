using ErabliereApi.Donnees.Action.Post;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Donnee : IIdentifiable<int?, Donnee>
    {
        /// <summary>
        /// L'id de l'occurence
        /// </summary>
        public int? Id { get; set; }

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
        public int? Iddp { get; set; }

        /// <inheritdoc />
        public int CompareTo([AllowNull] Donnee other)
        {
            if (other == default)
            {
                return 1;
            }

            if (D.HasValue == false)
            {
                return other.D.HasValue ? -1 : 0;
            }

            return D.Value.CompareTo(other.D);
        }

        public bool IdentiqueMemeLigneDeTemps(PostDonnee donnee)
        {
            return donnee.NB == NB &&
                   donnee.T == T &&
                   donnee.V == V &&
                   D.HasValue && donnee.D.HasValue &&
                   D.Value < donnee.D.Value;
        }
    }
}
