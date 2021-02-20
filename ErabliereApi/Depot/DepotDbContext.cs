using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ErabliereApi.Depot
{
    /// <summary>
    /// Dépot de donnée utilisant la classe ErabliereDbContext comme dépôt de donnée
    /// </summary>
    /// <typeparam name="T">L'entité</typeparam>
    public class DepotDbContext<T> : Depot<T> where T : class, IIdentifiable<int?, T>
    {
        private readonly ErabliereDbContext _context;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="context"></param>
        public DepotDbContext(ErabliereDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void Ajouter(T donnee)
        {
            _context.Set<T>().Add(donnee);

            _context.SaveChanges();
        }

        /// <inheritdoc />
        public Task<bool> Contient(object id)
        {
            return _context.Set<T>().AnyAsync(t => Equals(t.Id, id));
        }

        /// <inheritdoc />
        public Task<bool> Contient(Expression<Func<T, bool>> predicat)
        {
            return _context.Set<T>().AnyAsync(predicat);
        }

        /// <inheritdoc />
        public IEnumerable<T> Lister()
        {
            return _context.Set<T>();
        }

        /// <inheritdoc />
        public IEnumerable<T> Lister(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        /// <inheritdoc />
        public void Modifier(T donnee)
        {
            _context.Set<T>().Update(donnee);

            _context.SaveChanges();
        }

        /// <inheritdoc />
        public void Supprimer(T donnee)
        {
            _context.Set<T>().Remove(donnee);

            _context.SaveChanges();
        }
    }
}
