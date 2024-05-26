import {Component, Input, OnChanges, OnDestroy, SimpleChanges} from '@angular/core';
import {Chart, TooltipItem} from 'chart.js';
import {ErabliereApi} from 'src/core/erabliereapi.service';
import {WeatherForecase} from 'src/model/weatherforecast';
import {Erabliere} from "../model/erabliere";

@Component({
    selector: 'weather-forecast',
    templateUrl: 'weather-forecast.component.html',
    standalone: true,
})
export class WeatherForecastComponent implements OnChanges, OnDestroy {
    @Input() erabliere?: Erabliere;

    chart: any;
    weatherData?: WeatherForecase;
    text?: string;
    error?: any;
    interval?: NodeJS.Timeout;

    constructor(private api: ErabliereApi) {
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes['erabliere']) {
            if (this.interval != null) {
                clearInterval(this.interval);
            }
            this.chart?.destroy();
            this.chart = null;
            if (this.erabliere?.id && this.erabliere?.codePostal) {
                this.getWeatherData();
                this.interval = setInterval(() => {
                    this.getWeatherData();
                }, 1000 * 60 * 60);
            }
        }
    }

    ngOnDestroy() {
        clearInterval(this.interval);
        this.chart?.destroy();
        this.chart = null;
    }

    getWeatherData() {
        this.api.getWeatherForecast(this.erabliere?.id).then((data: WeatherForecase) => {
            this.weatherData = data;
            this.error = null;
            this.createChart();
        }).catch((error: any) => {
            console.log(error);
            this.error = error;
            this.weatherData = undefined;
        });
    }

    convertToCelsius(fahrenheit: number) {
        return (fahrenheit - 32) * 5 / 9;
    }

    createChart() {
        if (this.weatherData == null) {
            return;
        }

        this.text = this.weatherData.Headline?.Text;

        const dataF = this.weatherData.DailyForecasts;
        const dates = dataF?.map(entry => entry.Date);
        const minTemperatures = dataF?.map(entry => this.convertToCelsius(entry.Temperature.Minimum.Value));
        const maxTemperatures = dataF?.map(entry => this.convertToCelsius(entry.Temperature.Maximum.Value));

        if (this.chart != null) {
            try {
                console.log("Try destroy previous chart");
                this.chart.destroy();
                this.chart = null;
            } catch (error) {
                console.log("Error destroy previous chart")
                console.log(error);
            }
        }

        setTimeout(() => {
            this.chart = new Chart('weatherChart', {
                type: 'line',
                data: {
                    labels: dates,
                    datasets: [
                        {
                            label: 'Maximum',
                            data: maxTemperatures,
                            borderColor: 'red',
                            fill: false,
                            pointRadius: 5,
                        },
                        {
                            label: 'Minimum',
                            data: minTemperatures,
                            borderColor: 'blue',
                            fill: false,
                            pointRadius: 5
                        },
                    ]
                },
                options: {
                    maintainAspectRatio: false,
                    aspectRatio: 1.7,
                    scales: {
                        x: {
                            type: 'time',
                            time: {
                                unit: 'day'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Temperature (°C)'
                            }
                        },
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    plugins: {
                        tooltip: {
                            callbacks: {
                                title: (tti: TooltipItem<"line">[]) => {
                                    const i = tti[0].dataIndex;
                                    let dt = this.weatherData?.DailyForecasts?.[i].Day.IconPhrase;
                                    let nt = this.weatherData?.DailyForecasts?.[i].Night.IconPhrase;
                                    return `Jour: ${dt} - Nuit: ${nt}`;
                                },
                                label: function (context: any) {
                                    var label = context.dataset.label || '';
                                    if (label) {
                                        label += ': ';
                                    }
                                    if (context.parsed.y !== null) {
                                        label += context.parsed.y.toFixed(2) + '°C';
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });
        }, 0);
    }
}
