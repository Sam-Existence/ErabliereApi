using ErabliereApi.Donnees.Action.Post;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Modèle donnée représente un trio de donnée. La température, le niveau de vaccium et le niveau d'un bassin
    /// </summary>
    public class Donnee : IIdentifiable<Guid?, Donnee>
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
        /// Temperature en dixi�me de celcius
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
        /// Id de l'érablière relié a cette donnée
        /// </summary>
        public Guid? IdErabliere { get; set; }

        /// <summary>
        /// L'erabliere relié à la donn�e
        /// </summary>
        public Erabliere? Erabliere { get; set; }

        /// <summary>
        /// Interval de date des données alimenté. Utiliser pour optimiser le nombre de données enregistrer
        /// 
        /// Plus grand interval d'alimentation de cette donnée, en seconde
        /// </summary>
        public int? PI { get; set; }

        /// <summary>
        /// Nombre d'occurence enrgistrer de cette donn�e
        /// </summary>
        public int Nboc { get; set; }

        /// <summary>
        /// Id donnée précédente
        /// </summary>
        public Guid? Iddp { get; set; }

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

            if (!other.D.HasValue)
            {
                return 1;
            }

            return D.Value.CompareTo(other.D.Value);
        }

        /// <summary>
        /// Indique si la donnée en paramètre est dans le future et possède les même valeur pour
        /// le niveau bassin, la température et le vaccium
        /// </summary>
        /// <param name="donnee">Une donnée</param>
        public bool IdentiqueMemeLigneDeTemps(PostDonnee donnee) => donnee.NB == NB &&
                                                                    donnee.T == T &&
                                                                    donnee.V == V &&
                                                                    D.HasValue && donnee.D.HasValue &&
                                                                    D.Value < donnee.D.Value;
    }
}
