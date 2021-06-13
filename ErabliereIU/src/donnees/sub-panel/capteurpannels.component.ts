import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Capteur } from 'src/model/capteur';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'capteur-panels',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6">
              <div *ngFor="let capteur of capteurs">
                <graph-panel [titre]="capteur.nom" 
                             [valeurActuel]="valeurActuelOf(capteur)"
                             [symbole]="symboleOf(capteur)"
                             [timeaxes]="timeaxeOf(capteur)" 
                             [datasets]="datasetOf(capteur)"></graph-panel>
              </div>
            </div>
          </div>
        </div>
    `
})
export class CapteurPannelsComponent implements OnInit {
  @Input() erabliere?: Erabliere;

  capteurs?: Capteur[];

  ngOnInit(): void {
    this.capteurs = this.erabliere?.capteurs;
  }

  timeaxeOf(capteur:Capteur): Label[] {
    return [];
  }

  datasetOf(capteur:Capteur): ChartDataSets[] {
    return[];
  }

  valeurActuelOf(capteur: Capteur) : string {
    if (capteur.donnees != undefined && capteur.donnees != null) {
      return capteur.donnees[capteur.donnees.length - 1].valeur?.toString() || "";
    }

    return "";
  }

  symboleOf(capteur: Capteur) : string {
    return capteur.symbole || "";
  }
}