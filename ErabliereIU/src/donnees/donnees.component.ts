import { Component } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
    selector: 'donnees-panel',
    template: `
        <div class="border-top">
            <h3>Donnees</h3>
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
export class DonneesComponent {
      lineChartData: ChartDataSets[] = [];
    
      lineChartLabels: Label[] = [];
    
      lineChartOptions = {
        responsive: true,
      };
    
      lineChartColors: Color[] = [
        {
          borderColor: 'black',
          backgroundColor: 'rgba(255,255,0,0.28)',
        },
      ];
    
      lineChartLegend = true;
      lineChartPlugins = [];
      lineChartType = 'line';

    constructor(){
        fetch("http://localhost:5000/erablieres/0/Donnees?dd=2021-03-15&df=2021-03-15T00:05:00")
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