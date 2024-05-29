import { Component, Input } from '@angular/core';
import { Capteur } from 'src/model/capteur';
import { GraphPannelComponent } from './graph-pannel.component';
import { WeatherForecastComponent } from '../weather-forecast.component';
import { Erabliere } from 'src/model/erabliere';
import { ImagePanelComponent } from './image-pannel.component';

@Component({
    selector: 'capteur-pannels',
    template: `
      <div class="row border-top">
          @for (capteur of capteurs; track capteur.id) {
          <div class="col-md-6">
              <graph-pannel [titre]="capteur.nom"
                            [symbole]="capteur.symbole"
                            [idCapteur]="capteur.id"
                            [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-pannel>
          </div>
          }
      </div>
    `,
    standalone: true,
    imports: [GraphPannelComponent, WeatherForecastComponent, ImagePanelComponent]
})
export class CapteurPannelsComponent {
  @Input() capteurs?: Capteur[]
  @Input() erabliere?: Erabliere
}
