using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErabliereApi.Donnees.Generic
{
    public class Pair<K, V>
    {
        public Pair(K id, V valeur)
        {
            Id = id;
            Valeur = valeur;
        }

        public K Id { get; set; }

        public V Valeur { get; set; }
    }
}
