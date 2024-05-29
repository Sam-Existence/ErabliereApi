import { type Meta, type StoryObj } from '@storybook/angular';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { WeatherForecastComponent } from 'src/donnees/weather-forecast.component';
import { Unit } from 'src/model/weatherForecast';

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
  args: {

  }
};

export const Erreur: Story = {
    args: {
      error: "Erreur de connexion"
    }
  };

export const Forecast5Days: Story = {
  args: {
    text: "Neige continuant jusqu’à demain après-midi; la tempête totalisera de 4 à 8 pouces",
    weatherData: {
        "headline": {
            "effectiveDate": "2023-11-27T01:00:00-05:00",
            "effectiveEpochDate": 1701064800,
            "severity": 2,
            "text": "Snow continuing through tomorrow afternoon with a storm total of 4-8 inches",
            "category": "snow",
            "endDate": "2023-11-28T19:00:00-05:00",
            "endEpochDate": 1701216000,
            "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?lang=en-us",
            "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?lang=en-us"
        },
        "dailyForecasts": [
            {
                "date": "2023-11-27T07:00:00-05:00",
                "epochDate": 1701086400,
                "temperature": {
                    "minimum": {
                        "value": 22.0,
                        "unit": Unit.F,
                        "unitType": 18
                    },
                    "maximum": {
                        "value": 39.0,
                        "unit": Unit.F,
                        "unitType": 18
                    }
                },
                "day": {
                    "icon": 22,
                    "iconPhrase": "Snow",
                    "hasPrecipitation": true,
                    "precipitationType": "Mixed",
                    "precipitationIntensity": "Moderate"
                },
                "night": {
                    "icon": 19,
                    "iconPhrase": "Flurries",
                    "hasPrecipitation": true,
                    "precipitationType": "Snow",
                    "precipitationIntensity": "Light"
                },
                "sources": [
                    "AccuWeather"
                ],
                "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=1&lang=en-us",
                "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=1&lang=en-us"
            },
            {
                "date": "2023-11-28T07:00:00-05:00",
                "epochDate": 1701172800,
                "temperature": {
                    "minimum": {
                        "value": 15.0,
                        "unit": Unit.F,
                        "unitType": 18
                    },
                    "maximum": {
                        "value": 27.0,
                        "unit": Unit.F,
                        "unitType": 18
                    }
                },
                "day": {
                    "icon": 19,
                    "iconPhrase": "Flurries",
                    "hasPrecipitation": true,
                    "precipitationType": "Snow",
                    "precipitationIntensity": "Light"
                },
                "night": {
                    "icon": 43,
                    "iconPhrase": "Mostly cloudy w/ flurries",
                    "hasPrecipitation": true,
                    "precipitationType": "Snow",
                    "precipitationIntensity": "Light"
                },
                "sources": [
                    "AccuWeather"
                ],
                "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=2&lang=en-us",
                "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=2&lang=en-us"
            },
            {
                "date": "2023-11-29T07:00:00-05:00",
                "epochDate": 1701259200,
                "temperature": {
                    "minimum": {
                        "value": 16.0,
                        "unit": Unit.F,
                        "unitType": 18
                    },
                    "maximum": {
                        "value": 21.0,
                        "unit": Unit.F,
                        "unitType": 18
                    }
                },
                "day": {
                    "icon": 4,
                    "iconPhrase": "Intermittent clouds",
                    "hasPrecipitation": false
                },
                "night": {
                    "icon": 38,
                    "iconPhrase": "Mostly cloudy",
                    "hasPrecipitation": false
                },
                "sources": [
                    "AccuWeather"
                ],
                "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=3&lang=en-us",
                "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=3&lang=en-us"
            },
            {
                "date": "2023-11-30T07:00:00-05:00",
                "epochDate": 1701345600,
                "temperature": {
                    "minimum": {
                        "value": 30.0,
                        "unit": Unit.F,
                        "unitType": 18
                    },
                    "maximum": {
                        "value": 34.0,
                        "unit": Unit.F,
                        "unitType": 18
                    }
                },
                "day": {
                    "icon": 6,
                    "iconPhrase": "Mostly cloudy",
                    "hasPrecipitation": false
                },
                "night": {
                    "icon": 26,
                    "iconPhrase": "Freezing rain",
                    "hasPrecipitation": true,
                    "precipitationType": "Mixed",
                    "precipitationIntensity": "Moderate"
                },
                "sources": [
                    "AccuWeather"
                ],
                "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=4&lang=en-us",
                "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=4&lang=en-us"
            },
            {
                "date": "2023-12-01T07:00:00-05:00",
                "epochDate": 1701432000,
                "temperature": {
                    "minimum": {
                        "value": 21.0,
                        "unit": Unit.F,
                        "unitType": 18
                    },
                    "maximum": {
                        "value": 39.0,
                        "unit": Unit.F,
                        "unitType": 18
                    }
                },
                "day": {
                    "icon": 7,
                    "iconPhrase": "Cloudy",
                    "hasPrecipitation": false
                },
                "night": {
                    "icon": 36,
                    "iconPhrase": "Intermittent clouds",
                    "hasPrecipitation": false
                },
                "sources": [
                    "AccuWeather"
                ],
                "mobileLink": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=5&lang=en-us",
                "link": "http://www.accuweather.com/en/ca/saint-victor/g0m/daily-weather-forecast/45942_pc?day=5&lang=en-us"
            }
        ]
    }
  }
};
