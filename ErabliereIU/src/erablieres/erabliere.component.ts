import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ActivatedRoute } from '@angular/router';
import { Erabliere } from 'src/model/erabliere';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { Subject } from 'rxjs';
import { ImagePanelComponent } from 'src/donnees/sub-panel/image-pannel.component';
import { RappelsComponent } from 'src/rappel/rappels.component';
import { WeatherForecastComponent } from 'src/donnees/weatherforecast.component';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html',
    standalone: true,
    imports: [
      DonneesComponent,
      CapteurPannelsComponent,
      BarilsComponent,
      ImagePanelComponent,
      RappelsComponent,
      WeatherForecastComponent
    ]
})
export class ErabliereComponent implements OnInit, OnDestroy {
  idErabliereSelectionee?: any;
  erabliere?: Erabliere;
  resetErabliere: Subject<Erabliere> = new Subject<Erabliere>();

  intervalImages?: any;
  displayImages: boolean = false;

  constructor(private _api: ErabliereApi, private route: ActivatedRoute) { 
    this.route.paramMap.subscribe(params => {
      this.idErabliereSelectionee = params.get('idErabliereSelectionee');
      console.log('erabliere.component.paramMap ' + this.idErabliereSelectionee);
      if (this.idErabliereSelectionee) {
        this._api.getErabliere(this.idErabliereSelectionee).then((erabliere) => {
          console.log(erabliere);
          this.erabliere = erabliere;
          this.resetErabliere.next(erabliere);
        });
      }
    });
    this.resetErabliere.subscribe((erabliere) => {
      this.erabliere = erabliere;
    });
  }

  ngOnInit(): void {   
    this.intervalImages = setInterval(() => {
      console.log("DonneesComponent getImages", this.idErabliereSelectionee)
      this._api.getImages(this.idErabliereSelectionee, 1).then(images => {
        this.displayImages = images.length > 0;
      });
    }, 1000 * 60 * 10);

    console.log("DonneesComponent getImages", this.idErabliereSelectionee)
    this._api.getImages(this.idErabliereSelectionee, 1).then(images => {
      this.displayImages = images.length > 0;
    });
  }

  ngOnDestroy(): void {
    clearInterval(this.intervalImages);
  }
}
