import { Component, Input, OnInit } from '@angular/core';
import { AuthorisationService } from 'src/authorisation/authorisation-service.component';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { environment } from 'src/environments/environment';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'erablieres',
    template: `
    <div class="row">
        <div class="col-lg border-right" [hidden]="cacheMenuErabliere">
            <div class="list-group">
                <a *ngFor="let erabliere of erablieres"
                   href="#"
                   class="list-group-item list-group-item-action" 
                   [class.active]="erabliereSelectionnee === erabliere.id"
                   (click)="handleErabliereLiClick(erabliere.id)">{{erabliere.nom}}</a>
            </div>
        </div>
            <div class="col-lg-10 col-md-12" [hidden]="pageSelectionnee !== 0">
                <div *ngFor="let erabliere of erablieres" [hidden]="erabliereSelectionnee !== erabliere.id">
                <donnees-panel [erabliere]="erabliere"></donnees-panel>
                <barils-panel *ngIf="erabliere.afficherSectionBaril" [erabliere]="erabliere"></barils-panel>
            </div>
        </div>

        <alerte-page class="col-10" [alertes]="alertes" 
                                    [hidden]="pageSelectionnee !== 1"
                                    (click)="handleAlerteClick()"></alerte-page>

        <camera-page class="col-10" [hidden]="pageSelectionnee !== 2"></camera-page>
        
        <div class="container">
            <documentation class="col-12" [hidden]="pageSelectionnee !== 4"></documentation>
        <div>

        <div class="container">
            <apropos class="col-12" [hidden]="pageSelectionnee !== 3"></apropos>
        </div>

    </div>
    `
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