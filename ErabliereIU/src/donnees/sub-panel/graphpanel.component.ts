import { Component, Input, ViewChild  } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label, BaseChartDirective } from 'ng2-charts';

@Component({
    selector: 'graph-panel',
    template: `
        <div class="border-top">
            <h3>{{ titre }} {{ valeurActuel }} {{ symbole }}</h3>

            <div class="btn-group">
                <div class="dropdown show">
                    <a class="btn btn-secondary dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Durée {{ duree }}
                    </a>

                <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                    <a class="dropdown-item" href="#">12h</a>
                </div>
            </div>

            </div>
            <div class="chart-wrapper">
                <canvas baseChart class="chart"
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
    @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

    @Input() datasets: ChartDataSets[] = [];

    @Input() timeaxes: Label[] = [];

    @Input() lineChartType = 'line' as ChartType;

    @Input() lineScaleType = 'time'

    @Input() yScaleOption:any = undefined

    lineChartOptions:ChartOptions = {
        responsive: true,
        scales: {
            xAxes: [{
                type: this.lineScaleType,
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
    
    @Input() titre:string|undefined="";
    duree:string = "12h"
    @Input() valeurActuel?:string|null|number|undefined;
    @Input() symbole:string="";

    constructor() { this.chart = undefined; }
}