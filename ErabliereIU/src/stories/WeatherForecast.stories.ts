import { type Meta, type StoryObj } from '@storybook/angular';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { WeatherForecastComponent } from 'src/donnees/weatherforecast.component';
import { Unit } from 'src/model/weatherforecast';

const meta: Meta<WeatherForecastComponent> = {
  title: 'WeatherForecast',
  component: WeatherForecastComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<WeatherForecastComponent>;

export const Vide: Story = {
  render: (args: WeatherForecastComponent) => ({
    props: args,
  }),
  args: {
    
  }
};

export const Erreur: Story = {
    render: (args: WeatherForecastComponent) => ({
      props: args,
    }),
    args: {
      error: "Erreur de connexion"
    }
  };

export const Forecast5Days: Story = {
  render: (args: WeatherForecastComponent) => ({
    props: args,
  }),
  args: {
    text: "Neige continuant jusqu’à demain après-midi; la tempête totalisera de 4 à 8 pouces",
    weatherData: {
        "Headline": {
            "EffectiveDate": "2023-11-27T01:00:00-05:00",
            "EffectiveEpochDate": 1701064800,
            "Severity": 2,
            "Text": "Snow continuing through tomorrow afternoon with a storm total of 4-8 inches",
            "Category": "snow",
            "EndDate": "2023-11-28T19:00:00-05:00",
            "EndEpochDate": 1701216000,
            "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?lang=en-us",
            "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?lang=en-us"
        },
        "DailyForecasts": [
            {
                "Date": "2023-11-27T07:00:00-05:00",
                "EpochDate": 1701086400,
                "Temperature": {
                    "Minimum": {
                        "Value": 22.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    },
                    "Maximum": {
                        "Value": 39.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    }
                },
                "Day": {
                    "Icon": 22,
                    "IconPhrase": "Snow",
                    "HasPrecipitation": true,
                    "PrecipitationType": "Mixed",
                    "PrecipitationIntensity": "Moderate"
                },
                "Night": {
                    "Icon": 19,
                    "IconPhrase": "Flurries",
                    "HasPrecipitation": true,
                    "PrecipitationType": "Snow",
                    "PrecipitationIntensity": "Light"
                },
                "Sources": [
                    "AccuWeather"
                ],
                "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=1&lang=en-us",
                "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=1&lang=en-us"
            },
            {
                "Date": "2023-11-28T07:00:00-05:00",
                "EpochDate": 1701172800,
                "Temperature": {
                    "Minimum": {
                        "Value": 15.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    },
                    "Maximum": {
                        "Value": 27.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    }
                },
                "Day": {
                    "Icon": 19,
                    "IconPhrase": "Flurries",
                    "HasPrecipitation": true,
                    "PrecipitationType": "Snow",
                    "PrecipitationIntensity": "Light"
                },
                "Night": {
                    "Icon": 43,
                    "IconPhrase": "Mostly cloudy w/ flurries",
                    "HasPrecipitation": true,
                    "PrecipitationType": "Snow",
                    "PrecipitationIntensity": "Light"
                },
                "Sources": [
                    "AccuWeather"
                ],
                "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=2&lang=en-us",
                "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=2&lang=en-us"
            },
            {
                "Date": "2023-11-29T07:00:00-05:00",
                "EpochDate": 1701259200,
                "Temperature": {
                    "Minimum": {
                        "Value": 16.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    },
                    "Maximum": {
                        "Value": 21.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    }
                },
                "Day": {
                    "Icon": 4,
                    "IconPhrase": "Intermittent clouds",
                    "HasPrecipitation": false
                },
                "Night": {
                    "Icon": 38,
                    "IconPhrase": "Mostly cloudy",
                    "HasPrecipitation": false
                },
                "Sources": [
                    "AccuWeather"
                ],
                "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=3&lang=en-us",
                "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=3&lang=en-us"
            },
            {
                "Date": "2023-11-30T07:00:00-05:00",
                "EpochDate": 1701345600,
                "Temperature": {
                    "Minimum": {
                        "Value": 30.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    },
                    "Maximum": {
                        "Value": 34.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    }
                },
                "Day": {
                    "Icon": 6,
                    "IconPhrase": "Mostly cloudy",
                    "HasPrecipitation": false
                },
                "Night": {
                    "Icon": 26,
                    "IconPhrase": "Freezing rain",
                    "HasPrecipitation": true,
                    "PrecipitationType": "Mixed",
                    "PrecipitationIntensity": "Moderate"
                },
                "Sources": [
                    "AccuWeather"
                ],
                "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=4&lang=en-us",
                "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=4&lang=en-us"
            },
            {
                "Date": "2023-12-01T07:00:00-05:00",
                "EpochDate": 1701432000,
                "Temperature": {
                    "Minimum": {
                        "Value": 21.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    },
                    "Maximum": {
                        "Value": 39.0,
                        "Unit": Unit.F,
                        "UnitType": 18
                    }
                },
                "Day": {
                    "Icon": 7,
                    "IconPhrase": "Cloudy",
                    "HasPrecipitation": false
                },
                "Night": {
                    "Icon": 36,
                    "IconPhrase": "Intermittent clouds",
                    "HasPrecipitation": false
                },
                "Sources": [
                    "AccuWeather"
                ],
                "MobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=5&lang=en-us",
                "Link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=5&lang=en-us"
            }
        ]
    }
  }
};