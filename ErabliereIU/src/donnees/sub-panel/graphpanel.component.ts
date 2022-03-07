import { Component, Input, OnInit, ViewChild  } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label, BaseChartDirective } from 'ng2-charts';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
    selector: 'graph-panel',
    template: `
        <div class="border-top">
            <h3>{{ titre }} {{ valeurActuel }} {{ symbole }}</h3>

            <div class="container">
                <div class="col-6 btn-group">
                    <div class="dropdown show">
                        <a class="btn btn-secondary dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Durée {{ duree }}
                        </a>

                    <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                        <a class="dropdown-item" href="#">12h</a>
                    </div>
                </div>

                <div class="col-12">
                    <ajouter-donnee-capteur *ngIf="ajouterDonneeDepuisInterface" [idCapteur]="idCapteur" (needToUpdate)="updateDonneesCapteur($event)"></ajouter-donnee-capteur>
                    <h6>{{ textActuel }}</h6>
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
export class GraphPannelComponent implements OnInit {
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
    @Input() symbole:string|undefined;
    @Input() textActuel?:string|undefined|null;
    @Input() ajouterDonneeDepuisInterface:boolean = false;

    constructor(private _api:ErabliereApi) { this.chart = undefined; }

    @Input() idCapteur?: any;

    interval?:any

    ngOnInit(): void {
        if (this.idCapteur != null) {
            this.doHttpCall();

            this.interval = setInterval(() => {
                this.doHttpCall();
            }, 1000 * 60);
        }
    }

    ngOnDestroy() {
        clearInterval(this.interval);
    }

    updateDonneesCapteur(event:any) {
        this.doHttpCall();
    }

    dernierDonneeRecu?:string = undefined;
    ddr?:string = undefined;

    ids:Array<number> = []

    doHttpCall(): void {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        var xddr = null;
        if (this.dernierDonneeRecu != undefined) {
          xddr = this.dernierDonneeRecu.toString();
        }

        this._api.getDonneesCapteur(this.idCapteur, debutFiltre, finFiltre, xddr).then(resp => {
            const h = resp.headers;

            this.dernierDonneeRecu = h.get("x-dde")?.valueOf();
            this.ddr = h.get("x-ddr")?.valueOf();

            var json = resp.body;

            if (json == null) {
                console.log("donneeCapteur response body was null. Return immediatly");
                return;
            }

            let ids = json.map(ee => ee.id);

            let donnees = [
                { data: json.map(donneeCapteur => donneeCapteur.valeur != null ? donneeCapteur.valeur / 10 : null), label: this.titre }
            ];

            let timeaxes = json.map(donneeCapteur => donneeCapteur.d);

            if (json.length > 0) {
                var actualData = json[json.length - 1];
                var tva = actualData.valeur;
                this.valeurActuel = tva != null ? (tva / 10).toFixed(1) : null;
                this.textActuel = actualData.text;
            }

            if (h.has("x-ddr") && this.ddr != undefined && h.get("x-ddr")?.valueOf() == this.ddr) {
              
                if (ids.length > 0 && this.ids[this.ids.length - 1] === ids[0]) {
                  this.datasets[0].data?.pop();
                  this.timeaxes.pop();
  
                  this.datasets[0].data?.push(donnees[0].data.shift() as any);
                  this.timeaxes.push(timeaxes.shift() as any);
                }
                
                donnees[0].data.forEach(t => this.datasets[0].data?.push(t as any));
                timeaxes.forEach(t => this.timeaxes.push(t as any));
                ids.forEach((n: number) => this.ids.push(n));
  
                while (this.timeaxes.length > 0 &&
                       new Date(this.timeaxes[0].toString()) < new Date(debutFiltre)) {
                  this.timeaxes.shift();
                  this.datasets[0].data?.shift();
                  this.ids.shift();
                }
              }
              else {
                this.datasets = donnees;
                this.timeaxes = timeaxes as any[];
                this.ids = ids;
              }

              this.chart?.update();
        });
    }

    obtenirDebutFiltre() : Date {
        var twelve_hour = 1000 * 60 * 60 * 12;

        return new Date(Date.now() - twelve_hour);
      }
}