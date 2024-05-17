import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";
import { CapteurListComponent } from "./capteur-list.component";
import { AjouterCapteurComponent } from "./ajouter-capteur.component";
import { NgIf } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs";

@Component({
    selector: 'gestion-capteurs',
    templateUrl: 'gestion-capteurs.component.html',
    standalone: true,
    imports: [NgIf, AjouterCapteurComponent, CapteurListComponent]
})
export class GestionCapteursComponent implements OnInit {

    @Input() idErabliere?: any;
    @Input() capteurs: Capteur[] = [];
    afficherSectionAjouterCapteur: boolean = false;

    constructor (
        private _api: ErabliereApi,
        private _route: ActivatedRoute) {
    }
    async ngOnInit()  {
        this._route.paramMap.subscribe(params => {
            this.idErabliere = params.get("idErabliereSelectionee");
            this.getCapteurs();
        });
    }

    showAjouterCapteur() {
        this.afficherSectionAjouterCapteur = true;
    }

    hideAjouterCapteur() {
        this.afficherSectionAjouterCapteur = false;
    }

    async getCapteurs() {
        if(this.idErabliere) {
            this._api.getCapteurs(this.idErabliere).then(capteurs => {
                this.capteurs = capteurs;
            });
        }
    }
}
