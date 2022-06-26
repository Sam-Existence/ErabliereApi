using AutoFixture.Xunit2;

namespace ErabliereApi.Test.Autofixture;

public class AutoApiData : AutoDataAttribute
{
    public AutoApiData() : base(() =>
    {
        return ErabliereFixture.CreerFixture(modelOnly: false);
    })
    {

    }
}
