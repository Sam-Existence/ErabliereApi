import { Component, Input, OnInit } from '@angular/core';
import { Capteur } from 'src/model/capteur';

@Component({
    selector: 'capteur-pannels',
    template: `
        <div class="border-top">
          <div class="row">
            <div class="col-md-6" *ngFor="let capteur of capteurs">
                <graph-pannel [titre]="capteur.nom" 
                              [symbole]="capteur.symbole"
                              [idCapteur]="capteur.id"
                              [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-pannel>
            </div>
          </div>
        </div>
    `
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[]

  ngOnInit(): void { }
  
}