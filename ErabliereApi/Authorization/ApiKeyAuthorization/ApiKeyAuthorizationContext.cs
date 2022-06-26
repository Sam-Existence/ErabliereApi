using ErabliereApi.Donnees;

namespace ErabliereApi.Authorization
{
    /// <summary>
    /// Classe de contexte permettant de stocker certain information calculé
    /// </summary>
    public class ApiKeyAuthorizationContext
    {
        /// <summary>
        /// Indique si la requête doit être autorisé
        /// </summary>
        public bool Authorize { get; set; }

        /// <summary>
        /// La référence vers l'utilisateur
        /// </summary>
        public Donnees.Customer? Customer { get; set; }

        /// <summary>
        /// La référence vers les détails de la clé d'api
        /// </summary>
        public ApiKey? ApiKey { get; set; }
    }
}