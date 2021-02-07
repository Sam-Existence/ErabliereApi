using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Donnees
{
    public class Erablieres : IIdentifiable<int?, Erablieres>
    {
        /// <summary>
        /// L'id de l'érablière
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Le nom de l'érablière
        /// </summary>
        public string? Nom { get; set; }

        /// <inheritdoc />
        public int CompareTo([AllowNull] Erablieres other)
        {
            return string.Compare(Nom, other?.Nom);
        }
    }
}