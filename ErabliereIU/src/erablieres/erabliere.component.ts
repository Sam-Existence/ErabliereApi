import { Component, Input } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { CapteurListComponent } from './capteur-list.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ActivatedRoute } from '@angular/router';
import { Erabliere } from 'src/model/erabliere';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { Subject } from 'rxjs';
import { ImagePanelComponent } from 'src/donnees/sub-panel/image-pannel.component';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html',
    standalone: true,
    imports: [
      NgIf, 
      NgFor, 
      DonneesComponent,
      CapteurPannelsComponent,
      BarilsComponent,
      ImagePanelComponent
    ]
})
export class ErabliereComponent {
  idErabliereSelectionee?: any;
  erabliere?: Erabliere;
  resetErabliere: Subject<Erabliere> = new Subject<Erabliere>();


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
}
