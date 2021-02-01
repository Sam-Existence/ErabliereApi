namespace ErabliereApi.Donnees
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }
}
