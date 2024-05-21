import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";
import { CapteurListComponent } from "./capteur-list.component";
import { AjouterCapteurComponent } from "./ajouter-capteur.component";
import {AjouterCapteurImageComponent} from "./ajouter-capteur-image.component";
import { NgIf } from "@angular/common";

@Component({
    selector: 'gestion-capteurs',
    templateUrl: 'gestion-capteurs.component.html',
    standalone: true,
    imports: [NgIf, AjouterCapteurComponent, CapteurListComponent, AjouterCapteurImageComponent]
})
export class GestionCapteursComponent implements OnInit {
    @Input() idErabliereSelectionee?: any;
    capteurs: Capteur[] = [];
    afficherSectionAjouterCapteur: boolean = false;
    afficherSectionAjouterCapteurImage: boolean = false;

    constructor (
        private _api: ErabliereApi) {
    }
    async ngOnInit()  {
        await this.getCapteurs();
    }

    showAjouterCapteur() {
        this.afficherSectionAjouterCapteur = true;
    }

    hideAjouterCapteur() {
        this.afficherSectionAjouterCapteur = false;
    }

    async getCapteurs() {
        if(this.idErabliereSelectionee) {
            this._api.getCapteurs(this.idErabliereSelectionee).then(capteurs => {
                this.capteurs = capteurs;
            });
        }
    }
}
