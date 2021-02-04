namespace ErabliereApi.Donnees
{
    /// <summary>
    /// Interface pour le classe possédant un Id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifiable<T>
    {
        /// <summary>
        /// Identifiant unique
        /// </summary>
        T Id { get; set; }
    }
}
