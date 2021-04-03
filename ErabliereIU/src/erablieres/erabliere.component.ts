import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'erablieres',
    template: `
    <div class="row">
        <div class="col-lg border-right" [hidden]="cacheMenuErabliere">
            <div class="list-group">
                <a *ngFor="let erabliere of erablieres"
                   href="#"
                   class="list-group-item list-group-item-action" [class.active]="erabliereSelectionnee === erabliere.id"
                   (click)="handleErabliereLiClick(erabliere.id)">{{erabliere.nom}}</a>
            </div>
        </div>
        <div class="col-lg-10 col-md-12" [hidden]="pageSelectionnee !== 0">
            <div *ngFor="let erabliere of erablieres" [hidden]="erabliereSelectionnee !== erabliere.id">
            <donnees-panel [erabliere]="erabliere"></donnees-panel>
            <barils-panel  [erabliere]="erabliere"></barils-panel>
        </div>
        </div>
        <alerte-page class="col-10" [hidden]="pageSelectionnee !== 1"></alerte-page>
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
    erablieres:any;

    erabliereSelectionnee?:number;

    @Input() cacheMenuErabliere?:boolean;

    @Input() pageSelectionnee?:number = 0;

    constructor(){
        this.erabliereSelectionnee = undefined;
    }

    ngOnInit() {
        fetch(environment.apiUrl + "/erablieres")
            .then(e => e.json())
            .then(e => {
                this.erablieres = e

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
}