import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'donnees-panel',
    template: `
        <div class="border-top">
          <h3>Données</h3>
          <h6>Id érablière {{ erabliere.id }}</h6>
          <div class="row">
            <div class="col-4">
              <temperature-panel [timeaxes]="timeaxes" [temperature]="temperature"></temperature-panel>
            </div>
            <div class="col-4">
              <vaccium-panel [timeaxes]="timeaxes" [vaccium]="vaccium"></vaccium-panel>
            </div>
            <div class="col-4">
              <niveaubassin-panel [timeaxes]="timeaxes" [niveaubassin]="niveaubassin"></niveaubassin-panel>
            </div>
          </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      @Input() erabliere:any

      timeaxes: Label[] = [];

      temperature: ChartDataSets[] = [];
      vaccium: ChartDataSets[] = [];
      niveaubassin: ChartDataSets[] = [];

      constructor(){ }

      ngOnInit() {
        this.doHttpCall();
        setInterval(() => {
          this.doHttpCall();
        }, 1000 * 60);
      }

      doHttpCall() {
        fetch(environment.apiUrl + "/erablieres/" + this.erabliere.id + "/Donnees")
          .then(e => e.json())
          .then(e => {
            this.temperature = [
              { data: e.map((ee: { t: number; }) => ee.t), label: 'Temperature' }
            ];
            this.vaccium = [
              { data: e.map((ee: { v: number; }) => ee.v), label: 'Vaccium' }
            ];
            this.niveaubassin = [
              { data: e.map((ee: { nb: number; }) => ee.nb), label: 'Niveau bassin' }
            ];
            this.timeaxes = e.map((ee: { d: string;}) => ee.d);
          });
      }
}