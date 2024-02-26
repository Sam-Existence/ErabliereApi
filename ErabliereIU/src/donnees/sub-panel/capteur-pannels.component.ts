import { Component, Input, OnInit } from '@angular/core';
import { Capteur } from 'src/model/capteur';
import { GraphPannelComponent } from './graph-pannel.component';
import { NgFor, NgIf } from '@angular/common';
import { WeatherForecastComponent } from '../weatherforecast.component';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'capteur-pannels',
    template: `
      <div class="row">
          <weather-forecast 
            *ngIf="notNullOrWitespace(erabliere?.codePostal) && !erabliere?.afficherTrioDonnees"
            class="col-md-6"></weather-forecast>
          <div class="border-top col-md-6" *ngFor="let capteur of capteurs">
              <graph-pannel [titre]="capteur.nom" 
                            [symbole]="capteur.symbole"
                            [idCapteur]="capteur.id"
                            [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-pannel>
          </div>
      </div>
    `,
    standalone: true,
    imports: [NgIf, NgFor, GraphPannelComponent, WeatherForecastComponent]
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[]
  @Input() erabliere?: Erabliere

  ngOnInit(): void { }
  
  notNullOrWitespace(arg0?: string): any {
    if (arg0 == null)
        return false;
    return arg0.trim().length > 0;
  }
}