import { Component, EventEmitter, Input, OnInit, Output, SimpleChange, ViewChild } from '@angular/core';
import { ChartDataset, ChartOptions, ChartType, LinearScale, TickOptions } from 'chart.js';
import { BaseChartDirective, NgChartsModule } from 'ng2-charts';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { AjouterDonneeCapteurComponent } from '../../donneeCapteurs/ajouter-donnee-capteur.component';
import { NgIf } from '@angular/common';
import { DateTimeSelectorComponent } from './userinput/date-time-selector.component';

@Component({
    selector: 'vaccium-graph-pannel',
    templateUrl: './graph-pannel.component.html',
    standalone: true,
    imports: [
        DateTimeSelectorComponent,
        NgIf,
        AjouterDonneeCapteurComponent,
        NgChartsModule,
    ],
})
export class VacciumGraphPannelComponent implements OnInit {
    @ViewChild(BaseChartDirective) chart?: BaseChartDirective;
    @Input() datasets: ChartDataset[] = [];
    @Input() timeaxes: string[] = [];
    @Input() lineChartType = 'line' as ChartType;
    @Input() lineScaleType: 'time' = 'time'
    lineChartOptions: ChartOptions = {
        maintainAspectRatio: false,
        aspectRatio: 1.7,
        backgroundColor: 'rgba(255,255,0,0.28)',
        color: 'black',
        borderColor: 'black',
        scales: {
            x: {
                type: this.lineScaleType,
                time: {
                    unit: 'minute',
                    tooltipFormat: 'yyyy-MM-dd HH:mm:ss',
                    displayFormats: {
                        minute: 'dd MMM HH:mm'
                    },
                },
                ticks: {
                    autoSkip: true,
                    maxTicksLimit: 6,
                }
            },
            y: {
                min: 0,
                max: 30
            } 
        }
    };

    lineChartLegend = true;
    lineChartPlugins = [];

    @Input() titre: string | undefined = "";
    @Input() valeurActuel?: string | null | number | undefined;
    @Input() symbole: string | undefined;
    @Input() textActuel?: string | undefined | null;
    @Input() ajouterDonneeDepuisInterface: boolean = false;

    constructor(private _api: ErabliereApi) { this.chart = undefined; }

    @Input() idCapteur?: any;

    interval?: any

    ngOnInit(): void {
        if (this.idCapteur != null) {
            this.doHttpCall();

            this.interval = setInterval(() => {
                if (this.fixRange == false) {
                    this.doHttpCall();
                }
            }, 1000 * 60);
        }
    }

    ngOnDestroy() {
        clearInterval(this.interval);
    }

    updateDonneesCapteur(event: any) {
        this.cleanGraphComponentCache();
        this.doHttpCall();
    }

    dernierDonneeRecu?: string = undefined;
    ddr?: string = undefined;

    ids: Array<number> = []

    doHttpCall(): void {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        if (this.fixRange) {
            debutFiltre = this.dateDebutFixRange;
            finFiltre = this.dateFinFixRange;
        }

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

            let donnees: Array<ChartDataset> = [
                { 
                    data: json.map(donneeCapteur => donneeCapteur.valeur != null ? donneeCapteur.valeur / 10 : null), label: this.titre,
                    fill: true,
                    pointBackgroundColor: 'rgba(255,255,0,0.8)',
                    pointBorderColor: 'black',
                    tension: 0.5
                }
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

    duree: string = "12h"
    debutEnHeure: number = 12;

    obtenirDebutFiltre(): Date {
        var twelve_hour = 1000 * 60 * 60 * this.debutEnHeure;

        return new Date(Date.now() - twelve_hour);
    }

    @Output() updateGraphCallback = new EventEmitter();
    @Output() updateGraphUsingFixRangeCallback = new EventEmitter();

    updateGraph(days: number, hours: number): void {
        this.fixRange = false;
        if (this.idCapteur != null) {
            this.duree = "";

            if (days != 0) {
                this.duree = days + " jours";
            }

            if (hours != 0) {
                this.duree = this.duree + " " + hours + "h";
            }

            this.debutEnHeure = hours + (24 * days);

            this.cleanGraphComponentCache();

            this.doHttpCall();
        }
        else {
            // When this.idCapteur is null, we are in a component such as 'donnees.component.ts'
            this.updateGraphCallback.emit({ days, hours });
        }
    }

    private cleanGraphComponentCache() {
        this.dernierDonneeRecu = undefined;
        this.ddr = undefined;
        this.ids = [];
    }

    updateDuree(duree: string) {
        this.duree = duree;
    }

    fixRange: boolean = false;

    updateGraphUsingFixRange() {
        this.fixRange = true;
        if (this.idCapteur != null) {
            this.cleanGraphComponentCache();
            this.updateDuree(this.dateDebutFixRange + " - " + this.dateFinFixRange);
            this.doHttpCall();
        }
        else {
            // When this.idCapteur is null, we are in a component such as 'donnees.component.ts'
            var obj = {
                dateDebutFixRange: this.dateDebutFixRange,
                dateFinFixRange: this.dateFinFixRange
            }
            this.updateGraphUsingFixRangeCallback.emit(obj);
        }
    }

    dateDebutFixRange: any = undefined;
    dateFinFixRange: any = undefined;

    captureDateDebut($event: SimpleChange) {
        this.dateDebutFixRange = $event.currentValue;
    }

    captureDateFin($event: SimpleChange) {
        this.dateFinFixRange = $event.currentValue;
    }
}