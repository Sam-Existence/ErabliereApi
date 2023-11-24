import { Component, Input, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
  selector: 'weather-forecast',
  template: `
    <div>
        <div id="graph-pannel-weather-forecast" class="border-top">
            <h3>Prévision 5 jours</h3>
            <div class="chart-wrapper">
                <canvas id="weatherChart" width="400" height="200"></canvas>
            </div>
        </div>
    </div>
  `,
  standalone: true,
})
export class 
WeatherForecastComponent implements OnInit {
  chart: any;
  weatherData?: any;
  @Input() idErabliere: any;

  constructor(private api: ErabliereApi) { }

  ngOnInit() {
    this.getWeatherData();
  }

  getWeatherData() {
    this.api.getWeatherForecast(this.idErabliere).then((data: any) => {
      this.weatherData = data;
      this.createChart();
    });
  }

  convertToCelsius(fahrenheit: number) {
    return (fahrenheit - 32) * 5 / 9;
  }

  createChart() {
    if (this.weatherData == null) {
        return;
    }
    const dataF = this.weatherData.DailyForecasts as Array<any>;
    const dates = dataF.map(entry => entry.Date);
    const minTemperatures = dataF.map(entry => this.convertToCelsius(entry.Temperature.Minimum.Value));
    const maxTemperatures = dataF.map(entry => this.convertToCelsius(entry.Temperature.Maximum.Value));

    this.chart = new Chart('weatherChart', {
      type: 'line',
      data: {
        labels: dates,
        datasets: [
          {
            label: 'Min Temperature',
            data: minTemperatures,
            borderColor: 'blue',
            fill: false
          },
          {
            label: 'Max Temperature',
            data: maxTemperatures,
            borderColor: 'red',
            fill: false
          }
        ]
      },
      options: {
        maintainAspectRatio: false,
        aspectRatio: 1.7,
        scales: {
          x: {
            display: true,
            title: {
              display: true,
              text: 'Date'
            },
            time: {
                tooltipFormat: 'yyyy-MM-dd HH:mm:ss',
                displayFormats: {
                    day: 'DD MMM'
                }
            }
          },
          y: {
            display: true,
            title: {
              display: true,
              text: 'Temperature (°C)'
            }
          }
        }
      }
    });
  }
}
