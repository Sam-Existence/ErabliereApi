import { Component, Input, OnInit } from '@angular/core';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { Capteur } from 'src/model/capteur';
import { GraphPannelComponent } from './graphpanel.component';

@Component({
    selector: 'capteur-panels',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6" *ngFor="let capteur of capteurs">
                <graph-panel [titre]="capteur.nom" 
                             [symbole]="capteur.symbole"
                             [idCapteur]="capteur.id"
                             [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-panel>
            </div>
          </div>
        </div>
    `
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[];

  // dataList:Array<GraphPannelComponent> = new Array<GraphPannelComponent>();

  ngOnInit(): void {
    // this.capteurs?.forEach(capteur => {

    // });
  }
  
}