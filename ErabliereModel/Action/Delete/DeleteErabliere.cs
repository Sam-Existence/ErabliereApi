namespace ErabliereApi.Donnees.Action.Delete
{
    /// <summary>
    /// Modèle de suppression d'une erablière
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class DeleteErabliere<TId>
    {
        /// <summary>
        /// L'id de l'érablière à supprimer
        /// </summary>
        public TId? Id { get; set; }
    }
}
