import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'donnees-panel',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6">
              <div *ngFor="capteur in capteurs">
                <graph-panel [titre]="capteur.nom" 
                             [valeurActuel]="capteur.donnees[capteur.donnees.length-1].valeur"
                             [symbole]="°c"
                             [timeaxes]="timeaxes" 
                             [datasets]="temperature"></graph-panel>
              </div>
            </div>
          </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      @Input() erabliere?:Erabliere
      @Input() dureeDonneesRequete:any

      timeaxes: Label[] = [];
      timeaxes_dompeux: Label[] = []

      derniereDonneeRecu?:string = undefined;
      ddr?:string = undefined;
      dernierDompeuxRecu?:string = undefined;
      ddrDompeux?:string = undefined;

      ids:Array<number> = []
      idsDompeux:Array<number> = []

      titre_temperature = "Temperature"
      temperature: ChartDataSets[] = []
      temperatureValueActuel?:string|null
      temperatureSymbole:string = "°c"

      titre_vaccium = "Vaccium"
      vaccium: ChartDataSets[] = []
      vacciumValueActuel?:string|null
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
      niveauBassinValueActuel?:string|null
      niveauBassinSymbole:string = "%"

      titre_dompeux = "Dompeux"
      dompeux: ChartDataSets[] = []
      dompeux_line_type: ChartType = "bar"
      dompeux_chart_type:string = "bar"

      constructor(private _erabliereApi:ErabliereApi){ }

      ngOnInit() {
        if (this.erabliere?.afficherTrioDonnees == true) {
          this.doHttpCall();
        }
        if (this.erabliere?.afficherSectionDompeux == true) {
          this.doHttpCallDompeux();
        }

        setInterval(() => {
          if (this.erabliere?.afficherTrioDonnees == true) {
            this.doHttpCall();
          }
          if (this.erabliere?.afficherSectionDompeux == true) {
            this.doHttpCallDompeux();
          }
        }, 1000 * 60);
      }

      doHttpCallDompeux() {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        var xddr = null;
        if (this.dernierDompeuxRecu != undefined) {
          xddr = this.dernierDompeuxRecu.toString();
        }

        this._erabliereApi.getDompeux(this.erabliere?.id, debutFiltre, finFiltre, xddr)
        .then(resp=> {
          const h = resp.headers;

          this.dernierDompeuxRecu = h.get("x-dde")?.valueOf();
          this.ddrDompeux = h.get("x-ddr")?.valueOf();

          var e = resp.body;

          if (e == null) {
            return;
          }
        
            let idsDompeux = e.map(ee => ee.id);

            let dompeux = [
              { data: e.map(ee => (new Date(ee.df).getTime() - new Date(ee.dd).getTime()) / 1000), label: 'Durée en seconde' }
            ];

            let timeaxes_dompeux = e.map(ee => new Date(ee.t).toLocaleTimeString());

            if (h.has("x-ddr") && this.ddrDompeux != undefined && h.get("x-ddr")?.valueOf() == this.ddrDompeux) {
            
              if (idsDompeux.length > 0 && this.idsDompeux[this.idsDompeux.length - 1] === idsDompeux[0]) {
                dompeux[0].data.shift();
                timeaxes_dompeux.shift();
                idsDompeux.shift();
              }
              
              dompeux[0].data.forEach((d:number) => this.dompeux[0].data?.push(d));
              timeaxes_dompeux.forEach((t: Label) => this.timeaxes_dompeux?.push(t));
              idsDompeux.forEach((n: number) => this.idsDompeux.push(n));

              while (this.timeaxes_dompeux.length > 0 &&
                new Date(this.timeaxes_dompeux[0].toString()) < new Date(debutFiltre)) {
                this.timeaxes_dompeux.shift();
                this.dompeux[0].data?.shift();
                this.idsDompeux.shift();
              }
            }
            else {
              this.dompeux = dompeux;
              this.timeaxes_dompeux = timeaxes_dompeux;
              this.idsDompeux = idsDompeux;
            }
        });
      }

      doHttpCall() {
        let debutFiltre = this.obtenirDebutFiltre().toISOString();
        let finFiltre = new Date().toISOString();

        var xddr = null;
        if (this.derniereDonneeRecu != undefined) {
          xddr = this.derniereDonneeRecu.toString();
        }

        this._erabliereApi.getDonnees(this.erabliere?.id, debutFiltre, finFiltre, xddr)
          .then(resp => {
            var h = resp.headers;

            this.derniereDonneeRecu = h.get("x-dde")?.valueOf();
            this.ddr = h.get("x-ddr")?.valueOf();

            var e = resp.body;

            if (e == null) {
              return;
            }

            let ids = e.map(ee => ee.id);

            let temperature = [
              { data: e.map(ee => ee.t != null ? ee.t / 10 : null), label: 'Temperature' }
            ];
            let vaccium = [
              { data: e.map(ee => ee.v != null ? ee.v / 10 : null), label: 'Vaccium' }
            ];
            let niveaubassin = [
              { data: e.map(ee => ee.nb), label: 'Niveau bassin' }
            ];

            let timeaxes = e.map(ee => ee.d);

            if (e.length > 0) {
              var tva = e[e.length - 1].t;
              var vva = e[e.length - 1].v;
              this.temperatureValueActuel = tva != null ? (tva / 10).toFixed(1) : null;
              this.vacciumValueActuel = vva != null ? (vva / 10).toFixed(1) : null;
              this.niveauBassinValueActuel = e[e.length - 1].nb?.toString();
            }

            if (h.has("x-ddr") && this.ddr != undefined && h.get("x-ddr")?.valueOf() == this.ddr) {
              
              if (ids.length > 0 && this.ids[this.ids.length - 1] === ids[0]) {
                this.temperature[0].data?.pop();
                this.vaccium[0].data?.pop();
                this.niveaubassin[0].data?.pop();
                this.timeaxes.pop();

                this.temperature[0].data?.push(temperature[0].data.shift() as any);
                this.vaccium[0].data?.push(vaccium[0].data.shift() as any);
                this.niveaubassin[0].data?.push(niveaubassin[0].data.shift() as any);
                this.timeaxes.push(timeaxes.shift() as any);
              }
              
              temperature[0].data.forEach(t => this.temperature[0].data?.push(t as any));
              vaccium[0].data.forEach(v => this.vaccium[0].data?.push(v as any));
              niveaubassin[0].data.forEach(nb => this.niveaubassin[0].data?.push(nb as any));
              timeaxes.forEach(t => this.timeaxes.push(t as any));
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
              this.timeaxes = timeaxes as any[];
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