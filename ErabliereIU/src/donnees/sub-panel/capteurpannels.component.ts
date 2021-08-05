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
                             [symbole]="capteur.symbole"
                             [backendAction]="capteur?.id"></graph-panel>
              </div>
            </div>
          </div>
        </div>
    `
})
export class CapteurPannelsComponent implements OnInit {
  @Input() erabliere?: Erabliere;

  constructor(private _api: ErabliereApi) {
    
  }

  capteurs?: Capteur[];

  ngOnInit(): void {
    this.capteurs = this.erabliere?.capteurs;
  }
  
}