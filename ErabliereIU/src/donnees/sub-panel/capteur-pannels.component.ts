import { Component, Input, OnInit } from '@angular/core';
import { Capteur } from 'src/model/capteur';
import { GraphPannelComponent } from './graph-pannel.component';
import { NgFor } from '@angular/common';

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
    `,
    standalone: true,
    imports: [NgFor, GraphPannelComponent]
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[]

  ngOnInit(): void { }
  
}