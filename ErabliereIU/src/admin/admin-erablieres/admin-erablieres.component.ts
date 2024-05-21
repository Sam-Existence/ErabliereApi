import { Component, OnInit } from '@angular/core';
import { Erabliere } from "../../model/erabliere";
import { ErabliereApi } from "../../core/erabliereapi.service";
import { ErabliereListComponent } from "./erabliere-list/erabliere-list.component";

@Component({
  selector: 'admin-erablieres',
  standalone: true,
    imports: [
        ErabliereListComponent
    ],
  templateUrl: './admin-erablieres.component.html'
})
export class AdminErablieresComponent implements OnInit {
    erablieres: Erabliere[] = [];

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
        this.chargerErablieres();
    }

    chargerErablieres() {
        this._api.getErablieresAdminExpandAccess().then(erablieres => {
            this.erablieres = erablieres;
        }).catch(error => {
            this.erablieres = [];
            throw error;
        });
    }
}
