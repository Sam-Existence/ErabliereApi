import { Component, Input, OnInit } from '@angular/core';
import { Capteur } from 'src/model/capteur';
import { GraphPannelComponent } from './graph-pannel.component';
import { NgFor, NgIf } from '@angular/common';
import { WeatherForecastComponent } from '../weatherforecast.component';
import { Erabliere } from 'src/model/erabliere';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ImagePanelComponent } from './image-pannel.component';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'capteur-pannels',
    template: `
      <div class="row">
          <div class="border-top col-md-6" *ngFor="let capteur of capteurs">
              <graph-pannel [titre]="capteur.nom" 
                            [symbole]="capteur.symbole"
                            [idCapteur]="capteur.id"
                            [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-pannel>
          </div>
      </div>
    `,
    standalone: true,
    imports: [NgIf, NgFor, GraphPannelComponent, WeatherForecastComponent, ImagePanelComponent]
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[]
  @Input() erabliere?: Erabliere

  constructor(private api: ErabliereApi, private route: ActivatedRoute) { }

  ngOnInit(): void { 
    console.log('CapteurPannelsComponent.ngOnInit', this.erabliere?.id);
    
    this.route.paramMap.subscribe(params => {
      if (this.erabliere != null) {
        this.erabliere.id = params.get('idErabliereSelectionee');
      }
    });
  }

}