namespace ErabliereApi.Test.Autofixture;
public static class GetRandomExtension
{
    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        var random = new Random();

        var count = list.Count();

        return list.ElementAt(random.Next(0, count));
    }
}
