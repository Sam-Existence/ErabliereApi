import { CdkDrag, CdkDragEnd, CdkDragHandle } from '@angular/cdk/drag-drop';
import { Component, Input, ViewChild } from '@angular/core';
import { ChartDataset, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective, NgChartsModule } from 'ng2-charts';

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

            <div class="example-handle" cdkDragHandle>
                <svg width="32px" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M10 9h4V6h3l-5-5-5 5h3v3zm-1 1H6V7l-5 5 5 5v-3h3v-4zm14 2l-5-5v3h-3v4h3v3l5-5zm-9 3h-4v3H7l5 5 5-5h-3v-3z"></path>
                    <path d="M0 0h24v24H0z" fill="none"></path>
                </svg>
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
    `,
    standalone: true,
    imports: [NgChartsModule, CdkDrag, CdkDragHandle],
})
export class BarPannelComponent {
    @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

    @Input() datasets: ChartDataset[]

    @Input() timeaxes: string[]

    @Input() barChartType: ChartType;

    lineChartOptions: ChartOptions

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
            maintainAspectRatio: false,
            aspectRatio: 1.7,
            borderColor: 'black',
            backgroundColor: 'rgba(33,42,234,0.78)',
            scales: {
                x: {
                    grid: {
                        offset: true
                    }
                }
            }
        }

        this.lineChartLegend = true
        this.lineChartPlugins = []
        this.titre = ""
        this.duree = "12h"
        this.valeurActuel = ""
        this.symbole = ""
        this.chart = undefined
    }
    dragEnd(event: CdkDragEnd) {
        console.log(event);
    }
}