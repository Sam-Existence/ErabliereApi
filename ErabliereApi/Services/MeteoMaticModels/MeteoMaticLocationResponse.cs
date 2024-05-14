namespace ErabliereApi.Services.MeteoMaticModels;

public class MeteoMaticLocationResponse
{
    public string documentation { get; set; }

    public Licenses licenses { get; set; }

    public List<MeteoMaticLocationResult> result { get; set; }
}

public class Licenses
{
    public string? name { get; set; }
}

public class MeteoMaticLocationResult
{
    public Geometry geometry { get; set; }
}

public class Geometry
{
    public double lat { get; set; }

    public double lng { get; set; }
}