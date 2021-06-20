import { Component, Input, OnInit } from '@angular/core';
import { AuthorisationService } from 'src/authorisation/authorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { environment } from 'src/environments/environment';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html'
})
export class ErabliereComponent implements OnInit {
    erablieres?: Array<Erabliere>;

    erabliereSelectionnee?:number;

    @Input() cacheMenuErabliere?:boolean;

    @Input() pageSelectionnee?:number = 0;

    alertes?: Array<any>;

    constructor(private _erabliereApi: ErabliereApi){
        this.erabliereSelectionnee = undefined;
    }

    ngOnInit() {
        this._erabliereApi.getErablieres().then(erablieres => {
            this.erablieres = erablieres;

            if (this.erablieres.length > 0) {
                this.erabliereSelectionnee = this.erablieres[0].id;
            }
            else {
                // TODO : Aucun érablière trouvé
            }
        });
    }

    handleErabliereLiClick(idErabliere: number) {
        this.erabliereSelectionnee = idErabliere;
    }

    handleAlerteClick() {
        this.loadAlertes();
    }

    loadAlertes() {
        this._erabliereApi.getAlertes(this.erabliereSelectionnee).then(alertes => {
            this.alertes = alertes;
        });
    }
}