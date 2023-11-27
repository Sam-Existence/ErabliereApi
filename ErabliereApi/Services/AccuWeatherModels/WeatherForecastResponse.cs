
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
#nullable disable
public class WeatherForecastResponse
{
    public Headline Headline { get; set; }

    public Dailyforecast[] DailyForecasts { get; set; }
}

public class Headline
{
    public DateTime EffectiveDate { get; set; }
    public int EffectiveEpochDate { get; set; }
    public int Severity { get; set; }
    public string Text { get; set; }
    public string Category { get; set; }
    public DateTime EndDate { get; set; }
    public int EndEpochDate { get; set; }
    public string MobileLink { get; set; }
    public string Link { get; set; }
}

public class Dailyforecast
{
    public DateTime Date { get; set; }
    public int EpochDate { get; set; }
    public Temperature Temperature { get; set; }
    public Day Day { get; set; }
    public Night Night { get; set; }
    public string[] Sources { get; set; }
    public string MobileLink { get; set; }
    public string Link { get; set; }
}

public class Temperature
{
    public Minimum Minimum { get; set; }
    public Maximum Maximum { get; set; }
}

public class Minimum
{
    public float Value { get; set; }
    public string Unit { get; set; }
    public int UnitType { get; set; }
}

public class Maximum
{
    public float Value { get; set; }
    public string Unit { get; set; }
    public int UnitType { get; set; }
}

public class Day
{
    public int Icon { get; set; }
    public string IconPhrase { get; set; }
    public bool HasPrecipitation { get; set; }
    public string PrecipitationType { get; set; }
    public string PrecipitationIntensity { get; set; }
}

public class Night
{
    public int Icon { get; set; }
    public string IconPhrase { get; set; }
    public bool HasPrecipitation { get; set; }
    public string PrecipitationType { get; set; }
    public string PrecipitationIntensity { get; set; }
}
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
#nullable enable