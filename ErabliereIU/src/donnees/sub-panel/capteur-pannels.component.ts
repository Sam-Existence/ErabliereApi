import { NgClass, NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Capteur } from 'src/model/capteur';
import { Erabliere } from 'src/model/erabliere';
import { WeatherForecastComponent } from '../weatherforecast.component';
import { GraphPannelComponent } from './graph-pannel.component';
import { ImagePanelComponent } from './image-pannel.component';

@Component({
    selector: 'capteur-pannels',
    templateUrl: './capteur-pannels.component.html',
    standalone: true,
    imports: [NgIf, NgFor, GraphPannelComponent, WeatherForecastComponent, ImagePanelComponent, NgClass, MatButtonToggleModule, MatIconModule]
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs?: Capteur[] = []
  @Input() erabliere?: Erabliere
  idErabliereSelectionee?: any;

  public dimension?: string = "md-6";


  constructor(private api: ErabliereApi, private route: ActivatedRoute) { 
    this.route.paramMap.subscribe(params => {
      this.idErabliereSelectionee = params.get('idErabliereSelectionee');
    });
  }

  ngOnInit(): void {
    console.log('CapteurPannelsComponent.ngOnInit', this.erabliere?.id);

    this.route.paramMap.subscribe(params => {
      if (this.erabliere != null) {
        this.erabliere.id = params.get('idErabliereSelectionee');
      }
    });

    this.getDimensionCapteur();
  }

  changerDimension12() {
    this.dimension = "border-top col-md-12";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }
  changerDimension6() {
    this.dimension = "border-top col-md-6";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }
  changerDimension4() {
    this.dimension = "border-top col-md-3";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }

  getDimensionCapteur() {
    this.api.getCapteurs(this.idErabliereSelectionee).then((capteurs) => {
        this.dimension = capteurs[0].dimension;
    });
  }
}
