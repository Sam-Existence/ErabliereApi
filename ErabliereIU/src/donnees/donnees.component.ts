import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
    selector: 'donnees-panel',
    template: `
        <div class="border-top">
            <h3>Donnees</h3>
            <h6>Id érablière {{ erabliere.id }}</h6>
            <div class="chart-wrapper">
                <canvas baseChart 
                    [datasets]="lineChartData" 
                    [labels]="lineChartLabels" 
                    [options]="lineChartOptions"
                    [colors]="lineChartColors" 
                    [legend]="lineChartLegend" 
                    [chartType]="lineChartType" 
                    [plugins]="lineChartPlugins">
                </canvas>
            </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      lineChartData: ChartDataSets[] = [];
    
      lineChartLabels: Label[] = [];
    
      lineChartOptions = {
        responsive: true,
        scales: {
          xAxes: [{
            type: 'time',
            ticks: {
                autoSkip: true,
                maxTicksLimit: 7
            }
          }]
        }
      };
    
      lineChartColors: Color[] = [
        {
          borderColor: 'black',
          backgroundColor: 'rgba(255,255,0,0.28)',
        },
      ];
    
      lineChartLegend = true;
      lineChartPlugins = [];
      lineChartType = 'line' as ChartType;

      @Input() erabliere:any

      constructor(){ }

      ngOnInit() {
        this.doHttpCall();
        setInterval(() => {
          this.doHttpCall();
        }, 1000 * 60);
      }

      doHttpCall() {
        fetch("https://erabliereapi.freddycoder.com/erablieres/" + this.erabliere.id + "/Donnees")
          .then(e => e.json())
          .then(e => {
            this.lineChartData = [
              { data: e.map((ee: { nb: number; }) => ee.nb), label: 'Niveau bassin' },
              { data: e.map((ee: { t: number; }) => ee.t), label: 'Temperature' },
              { data: e.map((ee: { v: number; }) => ee.v), label: 'Vaccium' }
            ];

            this.lineChartLabels = e.map((ee: { d: string;}) => ee.d);
          });
      }
}