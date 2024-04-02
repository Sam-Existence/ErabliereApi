import { NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Capteur } from 'src/model/capteur';
import { Erabliere } from 'src/model/erabliere';
import { WeatherForecastComponent } from '../weatherforecast.component';
import { GraphPannelComponent } from './graph-pannel.component';
import { ImagePanelComponent } from './image-pannel.component';

@Component({
    selector: 'capteur-pannels',
    template: `
      <div class="row">
          <weather-forecast 
            *ngIf="notNullOrWitespace(erabliere?.codePostal) && !erabliere?.afficherTrioDonnees"
            class="col-md-6"></weather-forecast>
          <div *ngIf="displayImages" class="col-md-6">
            <image-panel [idErabliereSelectionnee]="erabliere?.id"></image-panel>
          </div>
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
        this.api.getImages(this.erabliere?.id, 1).then((images) => {
          this.displayImages = images.length > 0 && this.erabliere?.afficherTrioDonnees == false;
        });
      }
    });
  }

  displayImages: boolean = false;
  
  notNullOrWitespace(arg0?: string): any {
    if (arg0 == null)
        return false;
    return arg0.trim().length > 0;
  }
}