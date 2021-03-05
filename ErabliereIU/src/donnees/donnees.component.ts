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
              <bar-panel [titre]="titre_dompeux" 
                         [timeaxes]="timeaxes_dompeux" 
                         [datasets]="dompeux"
                         [barChartType]="dompeux_line_type"></bar-panel>
            <div>
          </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      @Input() erabliere:any
      @Input() dureeDonneesRequete:any

      timeaxes: Label[] = [];
      timeaxes_dompeux: Label[] = []

      derniereDonneeRecu?:string = undefined;
      ddr?:string = undefined;

      ids:Array<number> = []

      titre_temperature = "Temperature"
      temperature: ChartDataSets[] = []
      temperatureValueActuel:string = ""
      temperatureSymbole:string = "°c"

      titre_vaccium = "Vaccium"
      vaccium: ChartDataSets[] = []
      vacciumValueActuel:string = ""
      vacciumSymbole:string = "HG"
      vacciumYOption:any = [
          {
            display: true,
            ticks: { 
                beginAtZero: true, 
                max: 30
            }
        }
      ]

      titre_niveaubassin = "Niveau Bassin"
      niveaubassin: ChartDataSets[] = []
      niveauBassinValueActuel:string = ""
      niveauBassinSymbole:string = "%"

      titre_dompeux = "Dompeux"
      dompeux: ChartDataSets[] = []
      dompeux_line_type: ChartType = "bar"
      dompeux_chart_type:string = "bar"

      constructor(){ }

      ngOnInit() {
        this.doHttpCall();
        this.doHttpCallDompeux();
        setInterval(() => {
          this.doHttpCall(this.derniereDonneeRecu);
          this.doHttpCallDompeux();
        }, 1000 * 60);
      }

      doHttpCallDompeux() {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        fetch(environment.apiUrl + "/erablieres/" + this.erabliere.id + "/dompeux?dd=" + debutFiltre + "&df=" + finFiltre)
          .then(e => e.json())
          .then(e => {
              this.dompeux = [
                  { data: e.map((ee: { id: number, t: string, dd:string, df:string }) => (new Date(ee.df).getTime() - new Date(ee.dd).getTime()) / 1000), label: 'Durée en seconde' }
              ];

              this.timeaxes_dompeux = e.map((ee: { t: string;}) => new Date(ee.t).toLocaleTimeString());
          });
      }

      doHttpCall(derniereDonneeRecu:any = undefined) {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        var h = new Headers();
        if (this.derniereDonneeRecu != undefined) {
          h.append("x-ddr", this.derniereDonneeRecu.toString());
        }

        fetch(environment.apiUrl + "/erablieres/" + this.erabliere.id + "/Donnees?dd=" + debutFiltre + "&df=" + finFiltre, { headers: h })
          .then(e => {
            this.derniereDonneeRecu = e.headers.get("x-dde")?.valueOf();
            this.ddr = e.headers.get("x-ddr")?.valueOf();

            return e.json();
          })
          .then(e => {
            let ids = e.map((ee: { id: number; }) => ee.id);

            let temperature = [
              { data: e.map((ee: { t: number; }) => ee.t / 10), label: 'Temperature' }
            ];
            let vaccium = [
              { data: e.map((ee: { v: number; }) => ee.v / 10), label: 'Vaccium' }
            ];
            let niveaubassin = [
              { data: e.map((ee: { nb: number; }) => ee.nb), label: 'Niveau bassin' }
            ];

            let timeaxes = e.map((ee: { d: string;}) => new Date(ee.d));

            if (e.length > 0) {
              this.temperatureValueActuel = (e[e.length - 1].t / 10).toFixed(1);
              this.vacciumValueActuel = (e[e.length - 1].v / 10).toFixed(1);
              this.niveauBassinValueActuel = e[e.length - 1].nb;
            }

            if (h.has("x-ddr") && this.ddr != undefined && h.get("x-ddr")?.valueOf() == this.ddr) {
              
              if (ids.length > 0 && this.ids[this.ids.length - 1] === ids[0]) {
                this.temperature[0].data?.pop();
                this.vaccium[0].data?.pop();
                this.niveaubassin[0].data?.pop();
                this.timeaxes.pop();

                this.temperature[0].data?.push(temperature[0].data.shift());
                this.vaccium[0].data?.push(vaccium[0].data.shift());
                this.niveaubassin[0].data?.push(niveaubassin[0].data.shift());
                this.timeaxes.push(timeaxes.shift());
              }
              
              temperature[0].data.forEach((t:number) => this.temperature[0].data?.push(t));
              vaccium[0].data.forEach((v:number) => this.vaccium[0].data?.push(v));
              niveaubassin[0].data.forEach((nb:number) => this.niveaubassin[0].data?.push(nb));
              timeaxes.forEach((t: Label) => this.timeaxes.push(t));
              ids.forEach((n: number) => this.ids.push(n));

              while (this.timeaxes.length > 0 &&
                     new Date(this.timeaxes[0].toString()) < new Date(debutFiltre)) {
                this.timeaxes.shift();
                this.temperature[0].data?.shift();
                this.vaccium[0].data?.shift();
                this.niveaubassin[0].data?.shift();
                this.ids.shift();
              }
            }
            else {
              this.temperature = temperature;
              this.vaccium = vaccium;
              this.niveaubassin = niveaubassin;
              this.timeaxes = timeaxes;
              this.ids = ids;
            }
          })
          .catch(reason => {
            console.log(reason);
            this.derniereDonneeRecu = undefined;
            this.ddr = undefined;
          });
        }

      obtenirDebutFiltre() : Date {
        var twelve_hour = 1000 * 60 * 60 * 12;

        return new Date(Date.now() - twelve_hour);
      }
}