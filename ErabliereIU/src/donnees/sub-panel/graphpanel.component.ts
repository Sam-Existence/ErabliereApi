import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'graph-panel',
    template: `
        <div class="border-top">
            <h3>{{ titre }}</h3>
            <div class="chart-wrapper">
                <canvas baseChart 
                    [datasets]="datasets" 
                    [labels]="timeaxes" 
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
export class GraphPannelComponent {
    @Input() datasets: ChartDataSets[] = [];

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
        }
    ];

    lineChartLegend = true;
    lineChartPlugins = [];
    lineChartType = 'line' as ChartType;

    @Input() titre:string = "";

    constructor() { }
}