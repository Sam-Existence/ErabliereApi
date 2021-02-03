import { Component } from '@angular/core';

@Component({
    selector: 'donnees-panel',
    template: `
        <div style="width:75%;">
            <canvas id="canevas-donnees"></canvas>
        </div>
    `
})
export class DonneesComponent {
    constructor(){
        fetch("http://localhost:5000/erablieres/0/Donnees?dd=2021-03-15&df=2021-03-15T00:05:00")
            .then(e => e.json())
            .then(d => {
                var config = {
                    type: 'line',
                    data: {
                        labels: d.map(function (item:any) { return item.d; }),
                        datasets: [{
                            label: 'Temperature',
                            backgroundColor: window.chartColors.red,
                            borderColor: window.chartColors.red,
                            data: d.map(function (item:any) { return item.t }),
                            fill: false,
                        }, {
                            label: 'Vaccium',
                            backgroundColor: window.chartColors.blue,
                            borderColor: window.chartColors.blue,
                            data: d.map(function (item:any) { return item.v }),
                        }, {
                            label: 'Niveau bassin',
                            backgroundColor: window.chartColors.green,
                            borderColor: window.chartColors.green,
                            data: d.map(function (item:any) { return item.nb }),
                        }]
                    },
                    options: {
                        responsive: true,
                        title: {
                            display: true,
                            text: 'Visualisation donnée'
                        },
                        tooltips: {
                            mode: 'index',
                            intersect: false,
                        },
                        hover: {
                            mode: 'nearest',
                            intersect: true
                        },
                        scales: {
                            xAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Date'
                                }
                            }],
                            yAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Valeur'
                                }
                            }]
                        }
                    }
                };

                //var ctx = document.getElementById('canevas-donnees').getContext('2d');
                //window.myLine = new Chart(ctx, config);
            });
    }
}