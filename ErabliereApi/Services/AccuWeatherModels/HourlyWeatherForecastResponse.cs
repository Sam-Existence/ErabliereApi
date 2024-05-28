namespace ErabliereApi.Services.AccuWeatherModels;

public class HourlyWeatherForecastResponse
{
    public DateTime DateTime { get; set; }
    public int EpochDateTime { get; set; }
    public int WeatherIcon { get; set; }
    public string IconPhrase { get; set; }
    public bool HasPrecipitation { get; set; }
    public string PrecipitationType { get; set; }
    public string PrecipitationIntensity { get; set; }
    public HourlyForecastTemperature Temperature { get; set; }
    public string MobileLink { get; set; }
    public string Link { get; set; }
}

public class HourlyForecastTemperature
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public int UnitType { get; set; }
}