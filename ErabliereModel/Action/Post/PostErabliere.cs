using System.ComponentModel.DataAnnotations;

namespace ErabliereApi.Donnees.Action.Post
{
    public class PostErabliere
    {
        /// <summary>
        /// Le nom de l'érablière
        /// </summary>
        [MaxLength(50)]
        public string? Nom { get; set; }
    }
}
