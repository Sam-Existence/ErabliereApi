import { Component, OnInit } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ActivatedRoute } from '@angular/router';
import { Erabliere } from 'src/model/erabliere';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { Subject } from 'rxjs';
import { ImagePanelComponent } from 'src/donnees/sub-panel/image-pannel.component';
import { RappelsComponent } from 'src/rappel/rappels.component';
import { WeatherForecastComponent } from 'src/donnees/weather-forecast.component';

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
export class ErabliereComponent implements OnInit {
  idErabliereSelectionee?: any;
  erabliere?: Erabliere;
  resetErabliere: Subject<Erabliere> = new Subject<Erabliere>();

  displayImages: boolean = false;
  displayCapteurs: boolean = false;

  constructor(private _api: ErabliereApi, private route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.idErabliereSelectionee = params.get('idErabliereSelectionee');

      if (this.idErabliereSelectionee) {
        this._api.getErabliere(this.idErabliereSelectionee).then((erabliere) => {
          this.erabliere = erabliere;
          if (erabliere.capteurs?.length) {
              this.displayCapteurs = erabliere.capteurs?.length > 0;
          } else {
              this.displayCapteurs = false;
          }
          this.resetErabliere.next(erabliere);
        });
      }
    });
    this.resetErabliere.subscribe((erabliere) => {
      this.erabliere = erabliere;
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.idErabliereSelectionee = params.get('idErabliereSelectionee');
      if (this.idErabliereSelectionee) {
        this._api.getImages(this.idErabliereSelectionee, 1).then((images) => {
          this.displayImages = images.length > 0;
        });
      }
    });
  }
}
