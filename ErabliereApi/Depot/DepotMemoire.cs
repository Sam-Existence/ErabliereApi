using ErabliereApi.Donnees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ErabliereApi.Depot
{
    /// <summary>
    /// Implémentation en mémoire du dépôt de données
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DepotMemoire<T> : Depot<T> where T : class, IIdentifiable<int?, T>
    {
        private readonly SortedDictionary<int, T> _liste;
        private readonly Random _random;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public DepotMemoire()
        {
            _liste = new SortedDictionary<int, T>();
            _random = new Random();
        }

        /// <inheritdoc />
        public void Ajouter(T donnee)
        {
            if (donnee.Id == null)
            {
                donnee.Id = 1 + _liste.Count;

                while (_liste.ContainsKey(donnee.Id.Value))
                {
                    donnee.Id = _random.Next();
                }
            }
            else if (_liste.ContainsKey(donnee.Id.Value))
            {
                throw new InvalidOperationException($"L'id {donnee.Id} dans le dépôt {nameof(T)} existe déjà.");
            }

            _liste.Add(donnee.Id.Value, donnee);
        }

        /// <inheritdoc />  
        public Task<bool> Contient(object id)
        {
            return Task.FromResult(_liste.ContainsKey((int)id));
        }

        /// <inheritdoc />  
        public Task<bool> Contient(Expression<Func<T, bool>> predicat)
        {
            return Task.FromResult(_liste.Values.Any(predicat.Compile()));
        }

        /// <inheritdoc />  
        public IEnumerable<T> Lister()
        {
            return _liste.Values;
        }

        /// <inheritdoc />
        public IEnumerable<T> Lister(Func<T, bool> predicate)
        {
            return _liste.Values.Where(predicate);
        }

        /// <inheritdoc />
        public void Modifier(T donnee)
        {
            if (donnee == null)
            {
                throw new ArgumentNullException(nameof(donnee));
            }
            if (donnee.Id == null)
            {
                throw new ArgumentNullException($"{nameof(donnee)}.{nameof(donnee.Id)}");
            }

            _liste[donnee.Id.Value] = donnee;
        }

        /// <inheritdoc />
        public void Supprimer(T donnee)
        {
            if (donnee == null)
            {
                throw new ArgumentNullException(nameof(donnee));
            }
            if (donnee.Id == null)
            {
                throw new ArgumentNullException($"{nameof(donnee)}.{nameof(donnee.Id)}");
            }

            _liste.Remove(donnee.Id.Value);
        }
    }
}
