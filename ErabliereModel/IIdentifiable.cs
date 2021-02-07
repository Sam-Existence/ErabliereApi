using System;

namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Interface pour le classe possédant un Id
    /// </summary>
    /// <typeparam name="Tid"></typeparam>
    /// <typeparam name="Tcomp"></typeparam>
    public interface IIdentifiable<Tid, Tcomp> : IComparable<Tcomp>
    {
        /// <summary>
        /// Identifiant unique
        /// </summary>
        Tid Id { get; set; }
    }
}
