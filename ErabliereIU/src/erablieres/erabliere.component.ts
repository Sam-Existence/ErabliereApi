import { Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'erablieres',
    templateUrl: 'erabliere.component.html'
})
export class ErabliereComponent implements OnInit {
    erablieres?: Array<Erabliere>;

    erabliereSelectionnee?:Erabliere;

    idSelectionnee?:any

    @Input() cacheMenuErabliere?:boolean;

    @Input() pageSelectionnee?:number = 0;

    alertes?: Array<any>;

    private _authService: IAuthorisationSerivce

    constructor(private _erabliereApi: ErabliereApi, authFactory: AuthorisationFactoryService){
        this.erabliereSelectionnee = undefined;
        this._authService = authFactory.getAuthorisationService();
    }

    async ngOnInit() {
        this._authService.loginChanged.subscribe(loggedIn => {
            if (loggedIn) {
                this.ngOnInit();
            }
        });

        const erablieres = await this._erabliereApi.getErablieresExpandCapteurs();

        this.erablieres = erablieres.sort((a, b) => {
            if (a.indiceOrdre != null && b.indiceOrdre == null)
            {
                return -1;
            }
            else if (b.indiceOrdre != null && a.indiceOrdre == null)
            {
                return 1;
            }
            else if (a.indiceOrdre != null && b.indiceOrdre != null)
            {
                return a.indiceOrdre - b.indiceOrdre;
            }

            return a.nom?.localeCompare(b.nom ?? "") ?? 0;
        });

        if (this.erablieres.length > 0) {
            this.erabliereSelectionnee = this.erablieres[0];
            this.idSelectionnee = this.erabliereSelectionnee.id;
        }
        else {
            // TODO : Aucun érablière trouvé
        }
    }

    handleErabliereLiClick(idErabliere: number) {
        if (this.erablieres == null || this.erablieres == undefined) {
            return;
        }

        this.erabliereSelectionnee = this.erablieres.find(e => e.id === idErabliere);

        this.idSelectionnee = this.erabliereSelectionnee?.id;

        if (this.pageSelectionnee == 1) {
            this.loadAlertes();
        }
    }

    loadAlertes() {
        this._erabliereApi.getAlertes(this.erabliereSelectionnee?.id).then(alertes => {
            this.alertes = alertes;
        });
    }
}