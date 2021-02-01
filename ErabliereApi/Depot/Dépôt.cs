using ErabliereApi.Donnees;
using System.Collections.Generic;

namespace ErabliereApi.Depot
{
    /// <summary>
    /// Interface pour abstraire le dépôt de données réel derière l'api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Dépôt<T> where T : IIdentifiable<int?>
    {
        /// <summary>
        /// Lister les données
        /// </summary>
        /// <returns>Énumération des données</returns>
        IEnumerable<T> Lister();

        /// <summary>
        /// Ajouter une donnée
        /// </summary>
        /// <param name="donnee"></param>
        void Ajouter(T donnee);

        /// <summary>
        /// Modifier une donnée
        /// </summary>
        /// <param name="donnee"></param>
        void Modifier(T donnee);

        /// <summary>
        /// Supprimer une donnée
        /// </summary>
        /// <param name="donnee"></param>
        void Supprimer(T donnee);
    }
}
