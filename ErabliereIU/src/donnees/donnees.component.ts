import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'donnees-panel',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6">
              <graph-panel [titre]="titre_temperature" 
                           [valeurActuel]="temperatureValueActuel"
                           [symbole]="temperatureSymbole"
                           [timeaxes]="timeaxes" 
                           [datasets]="temperature"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_vaccium" 
                           [valeurActuel]="vacciumValueActuel"
                           [symbole]="vacciumSymbole"
                           [timeaxes]="timeaxes" 
                           [datasets]="vaccium"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_niveaubassin" 
                           [valeurActuel]="niveauBassinValueActuel"
                           [symbole]="niveauBassinSymbole"
                           [timeaxes]="timeaxes" 
                           [datasets]="niveaubassin"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_dompeux" 
                           [timeaxes]="timeaxes_dompeux" 
                           [datasets]="dompeux"></graph-panel>
            <div>
          </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      @Input() erabliere:any
      @Input() dureeDonneesRequete:any

      timeaxes: Label[] = [];
      timeaxes_dompeux: Label[] = [];

      titre_temperature = "Temperature"
      temperature: ChartDataSets[] = [];
      temperatureValueActuel:string = "";
      temperatureSymbole:string = "°c";
      titre_vaccium = "Vaccium"
      vaccium: ChartDataSets[] = [];
      vacciumValueActuel:string = "";
      vacciumSymbole:string = "HG";
      titre_niveaubassin = "Niveau Bassin";
      niveaubassin: ChartDataSets[] = [];
      niveauBassinValueActuel:string = "";
      niveauBassinSymbole:string = "%";
      titre_dompeux = "Dompeux"
      dompeux: ChartDataSets[] = [];

      constructor(){ }

      ngOnInit() {
        this.doHttpCall();
        this.doHttpCallDompeux();
        setInterval(() => {
          this.doHttpCall();
          this.doHttpCallDompeux();
        }, 1000 * 60);
      }

      doHttpCallDompeux() {
        fetch(environment.apiUrl + "/erablieres/" + this.erabliere.id + "/dompeux")
                .then(e => e.json())
                .then(e => {
                    this.dompeux = [
                        { data: e.map((ee: { id: number; }) => ee.id), label: 'Dompeux' }
                    ];

                    this.timeaxes_dompeux = e.map((ee: { t: string;}) => ee.t);
                });
      }

      doHttpCall() {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString()

        fetch(environment.apiUrl + "/erablieres/" + this.erabliere.id + "/Donnees?dd=" + debutFiltre + "&df=" + finFiltre)
          .then(e => e.json())
          .then(e => {
            this.temperature = [
              { data: e.map((ee: { t: number; }) => ee.t / 10), label: 'Temperature' }
            ];
            this.vaccium = [
              { data: e.map((ee: { v: number; }) => ee.v / 10), label: 'Vaccium' }
            ];
            this.niveaubassin = [
              { data: e.map((ee: { nb: number; }) => ee.nb), label: 'Niveau bassin' }
            ];

            this.timeaxes = e.map((ee: { d: string;}) => new Date(ee.d));

            if (e.length > 0) {
              this.temperatureValueActuel = (e[e.length - 1].t / 10).toFixed(1);
              this.vacciumValueActuel = (e[e.length - 1].v / 10).toFixed(1);
              this.niveauBassinValueActuel = e[e.length - 1].nb;
            }
          });
        }

      obtenirDebutFiltre() : Date {
        var twelve_hour = 1000 * 60 * 60 * 12;

        return new Date(Date.now() - twelve_hour);
      }
}