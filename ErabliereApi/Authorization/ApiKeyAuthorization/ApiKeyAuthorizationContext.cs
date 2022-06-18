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
    }
}