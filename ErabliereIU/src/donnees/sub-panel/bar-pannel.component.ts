import { Component, Input, ViewChild } from '@angular/core';
import { ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
    selector: 'bar-pannel',
    template: `
        <div class="border-top">
            <h3>{{ titre }} {{ valeurActuel }} {{ symbole }}</h3>

            <div class="btn-group">
                <div class="dropdown show">
                    <a class="btn btn-secondary dropdown-toggle" 
                       href="#" role="button" 
                       id="dropdownMenuLink" 
                       data-toggle="dropdown" 
                       aria-haspopup="true" 
                       aria-expanded="false">
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
                    [legend]="lineChartLegend" 
                    [plugins]="lineChartPlugins"
                    [type]="barChartType">
                </canvas>
            </div>
        </div>
    `
})
export class BarPannelComponent {
    @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

    @Input() datasets: ChartDataset[]

    @Input() timeaxes: string[]

    @Input() barChartType: ChartType;

    lineChartOptions: ChartOptions

    lineChartColors: any[]

    lineChartLegend: boolean
    lineChartPlugins: never[]
    
    @Input() titre:string
    duree:string
    @Input() valeurActuel:string
    @Input() symbole:string

    constructor() {
        this.datasets = []
        this.timeaxes = []
        this.barChartType = 'bar' as ChartType
        this.lineChartOptions = {
            responsive: true,
            scales: {
                x: {
                    grid: {
                        offset: true
                    }
                }
            }
        }
        this.lineChartColors  = [
            {
                borderColor: 'black',
                backgroundColor: 'rgba(33,42,234,0.78)',
            }
        ]
        this.lineChartLegend = true
        this.lineChartPlugins = []
        this.titre = ""
        this.duree = "12h"
        this.valeurActuel = ""
        this.symbole = ""
        this.chart = undefined
    }
}