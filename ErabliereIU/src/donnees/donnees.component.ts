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
              <graph-panel [titre]="titre_temperature" [timeaxes]="timeaxes" [datasets]="temperature"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_vaccium" [timeaxes]="timeaxes" [datasets]="vaccium"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_niveaubassin"  [timeaxes]="timeaxes" [datasets]="niveaubassin"></graph-panel>
            </div>
            <div class="col-md-6">
              <graph-panel [titre]="titre_dompeux" [timeaxes]="timeaxes_dompeux" [datasets]="dompeux"></graph-panel>
            <div>
          </div>
        </div>
    `
})
export class DonneesComponent implements OnInit {
      @Input() erabliere:any

      timeaxes: Label[] = [];
      timeaxes_dompeux: Label[] = [];

      titre_temperature = "Temperature"
      temperature: ChartDataSets[] = [];
      titre_vaccium = "Vaccium"
      vaccium: ChartDataSets[] = [];
      titre_niveaubassin = "Niveau Bassin"
      niveaubassin: ChartDataSets[] = [];
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