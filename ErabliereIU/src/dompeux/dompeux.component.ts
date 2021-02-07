import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
    selector: 'dompeux-panel',
    template: `
        <div class="border-top">
            <h3>Dompeux</h3>
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
export class DompeuxComponent implements OnInit {
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
    lineChartType = 'line' as ChartType;

    @Input() erabliere:any

    constructor() { }

    ngOnInit() {
        fetch("http://localhost:5000/erablieres/" + this.erabliere.id + "/dompeux")
            .then(e => e.json())
            .then(e => {
                this.lineChartData = [
                    { data: e.map((ee: { id: number; }) => ee.id), label: 'Dompeux' }
                ];

                this.lineChartLabels = e.map((ee: { t: string;}) => ee.t);
            });
    }
}