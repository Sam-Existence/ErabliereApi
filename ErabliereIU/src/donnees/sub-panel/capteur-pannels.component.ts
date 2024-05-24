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
    template: `
      <div class="row">
          <div [ngClass]="Dimension" *ngFor="let capteur of capteurs">
            <div class="justify-content-end d-flex">
              <mat-button-toggle-group name="fontStyle" aria-label="Font Style">
                <mat-button-toggle id="gros" (click)="changerDimension12()">gros</mat-button-toggle>
                <mat-button-toggle id="normale" (click)="changerDimension6()">normale</mat-button-toggle>
                <mat-button-toggle id="petit" (click)="changerDimension4()">petit</mat-button-toggle>
              </mat-button-toggle-group>
            </div>
              <graph-pannel [titre]="capteur.nom"
                            [symbole]="capteur.symbole"
                            [idCapteur]="capteur.id"
                            [ajouterDonneeDepuisInterface]="capteur.ajouterDonneeDepuisInterface"></graph-pannel>
          </div>
      </div>
    `,
    standalone: true,
    imports: [NgIf, NgFor, GraphPannelComponent, WeatherForecastComponent, ImagePanelComponent, NgClass, MatButtonToggleModule, MatIconModule]
})
export class CapteurPannelsComponent implements OnInit {
  @Input() capteurs: Capteur[] = []
  @Input() erabliere?: Erabliere

  public Dimension: string = "border-top col-md-6";


  constructor(private api: ErabliereApi, private route: ActivatedRoute) { }

  ngOnInit(): void {
    console.log('CapteurPannelsComponent.ngOnInit', this.erabliere?.id);

    this.route.paramMap.subscribe(params => {
      if (this.erabliere != null) {
        this.erabliere.id = params.get('idErabliereSelectionee');
      }
    });
  }

  changerDimension12() {
    this.Dimension = "border-top col-md-12";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.Dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }
  changerDimension6() {
    this.Dimension = "border-top col-md-6";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.Dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }
  changerDimension4() {
    this.Dimension = "border-top col-md-4";
    for (let capteur of this.capteurs!) {
      capteur.dimension = this.Dimension;
      this.api.putCapteur(this.erabliere!.id, capteur)
    }
  }
}
