import { Component, Input } from '@angular/core';
import { ChartDataSets, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
  selector: 'niveaubassin-panel',
    template: `
        <div class="chart-wrapper">
            <canvas baseChart 
                [datasets]="niveaubassin" 
                [labels]="timeaxes" 
                [options]="lineChartOptions"
                [colors]="lineChartColors" 
                [legend]="lineChartLegend" 
                [chartType]="lineChartType" 
                [plugins]="lineChartPlugins">
            </canvas>
        </div>
    `
})
export class NiveauBassinCompoenent {
    @Input() niveaubassin: ChartDataSets[] = [];
    
    @Input() timeaxes: Label[] = [];
    
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
}