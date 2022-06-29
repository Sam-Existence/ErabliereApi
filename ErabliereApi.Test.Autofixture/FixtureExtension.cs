using AutoFixture;
using System.Net;

namespace ErabliereApi.Test.Autofixture;

public static class FixtureExtension
{
    public static IPAddress CreateRandomIPAddress(this IFixture fixture)
    {
        return new IPAddress(fixture.CreateMany<byte>(4).ToArray());
    }
}
