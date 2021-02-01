using ErabliereApi.Donnees;
using System.Collections.Generic;

namespace ErabliereApi.Depot
{
    /// <summary>
    /// Implémentation en mémoire du dépôt de données
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DépôtMemoire<T> : Dépôt<T> where T : IIdentifiable<int?>
    {
        private readonly List<T> _liste;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public DépôtMemoire()
        {
            _liste = new List<T>();
        }

        /// <inheritdoc />
        public void Ajouter(T donnee)
        {
            _liste.Add(donnee);
        }

        /// <inheritdoc />
        public IEnumerable<T> Lister()
        {
            return _liste;
        }

        /// <inheritdoc />
        public void Modifier(T donnee)
        {
            _liste[_liste.FindIndex(d => d.Id == donnee.Id)] = donnee;
        }

        /// <inheritdoc />
        public void Supprimer(T donnee)
        {
           _liste.RemoveAt(_liste.FindIndex(d => d.Id == donnee.Id));
        }
    }
}
