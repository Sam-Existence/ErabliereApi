using ErabliereApi.Donnees;

namespace ErabliereApi.Donnees
{
    public class Erablieres : IIdentifiable<int?>
    {
        public int? Id { get; set; }

        public string Nom { get; set; }
    }
}