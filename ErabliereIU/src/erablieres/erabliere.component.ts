import { Component, Input, OnInit } from '@angular/core';
import { GraphiqueComponent } from '../graphique/graphique.component';
import { NgIf, NgFor } from '@angular/common';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';
import { Router } from '@angular/router';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html',
    standalone: true,
    imports: [
      NgIf, 
      NgFor, 
      GraphiqueComponent
    ]
})
export class ErabliereComponent implements OnInit {
  @Input() idErabliereSelectionee?: any;
  erabliere?: Erabliere

  constructor(private _api: ErabliereApi, private router: Router) { 
    
  }

  async ngOnInit() {
    console.log("idErabliereSelectionee: " + this.idErabliereSelectionee);

    var erablieres = await this._api.getErablieresExpandCapteurs(false);

    this.erabliere = erablieres.find(e => e.id == this.idErabliereSelectionee);

    this.router.events.subscribe(async (val) => {
      if (this.idErabliereSelectionee != null) {
        console.log("routerEvent: idErabliereSelectionee: " + this.idErabliereSelectionee);

        var erablieres = await this._api.getErablieresExpandCapteurs(false);

        this.erabliere = erablieres.find(e => e.id == this.idErabliereSelectionee);
      }
    });
  }
}
