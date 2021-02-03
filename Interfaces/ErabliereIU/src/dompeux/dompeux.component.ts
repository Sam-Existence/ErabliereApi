import { Component } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
    selector: 'dompeux-panel',
    template: `
        <div class="border-top">
            <h3>Dompeux</h3>
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
export class DompeuxComponent {
    lineChartData: ChartDataSets[] = [
        { data: [85, 72, 78, 75, 77, 75], label: 'Crude oil prices' },
    ];

    lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June'];

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

    constructor() {
        fetch("http://localhost:5000/erablieres/0/dompeux?dd=2021-03-15&df=2021-03-15T00:05:00")
            .then(e => e.json());
    }
}