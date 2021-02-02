using ErabliereApi.Donnees;
using System;
using System.Collections.Generic;

namespace ErabliereApi.Depot
{
    /// <summary>
    /// Interface pour abstraire le dépôt de données réel derière l'api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface Depot<T>
    {
        /// <summary>
        /// Permet de vérifier si le dépôt contient une donnée avec l'id
        /// </summary>
        /// <param name="id">L'id a vérifier</param>
        /// <returns></returns>
        bool Contient(object id);

        /// <summary>
        /// Indique s'il existe une donnée selon le predicat
        /// </summary>
        /// <param name="predicat"></param>
        /// <returns></returns>
        bool Contient(Func<T, bool> predicat);

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <returns>Énumération des données</returns>
        IEnumerable<T> Lister();

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <returns>Énumération des données</returns>
        IEnumerable<T> Lister(Func<T, bool> predicate);

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
