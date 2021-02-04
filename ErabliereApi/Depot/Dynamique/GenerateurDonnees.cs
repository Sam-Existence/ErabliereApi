using ErabliereApi.Donnees;
using System;
using System.Collections.Generic;

namespace ErabliereApi.Depot.Dynamique
{
    public class GenerateurDonnees : Depot<Donnee>
    {
        public void Ajouter(Donnee donnee)
        {
            throw new NotImplementedException();
        }

        public bool Contient(object id)
        {
            throw new NotImplementedException();
        }

        public bool Contient(Func<Donnee, bool> predicat)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Donnee> Lister()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Donnee> Lister(Func<Donnee, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Modifier(Donnee donnee)
        {
            throw new NotImplementedException();
        }

        public void Supprimer(Donnee donnee)
        {
            throw new NotImplementedException();
        }
    }
}
