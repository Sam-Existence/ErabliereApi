import {Component, Input, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";
import { CapteurListComponent } from "./capteur-list.component";
import { AjouterCapteurComponent } from "./ajouter-capteur.component";
import {AjouterCapteurImageComponent} from "./ajouter-capteur-image.component";
import {CapteurImage} from "../model/capteurImage";
import {CapteurImageListComponent} from "./capteur-image-list.component";

@Component({
    selector: 'gestion-capteurs',
    templateUrl: 'gestion-capteurs.component.html',
    standalone: true,
    imports: [AjouterCapteurComponent, CapteurListComponent, AjouterCapteurImageComponent, CapteurImageListComponent]
})
export class GestionCapteursComponent implements OnChanges {
    @Input() idErabliereSelectionee?: any;
    capteurs: Capteur[] = [];
    capteursImage: CapteurImage[] = [];
    afficherSectionAjouterCapteur: boolean = false;
    afficherSectionAjouterCapteurImage: boolean = false;

    constructor (
        private _api: ErabliereApi) {
    }

    async ngOnChanges(changes: SimpleChanges): Promise<void> {
        if (changes.idErabliereSelectionee) {
            await this.getCapteurs();
            await this.getCapteursImage();
        }
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

    async getCapteursImage() {
        if(this.idErabliereSelectionee) {
            this._api.getCapteursImage(this.idErabliereSelectionee).then(capteurs => {
                this.capteursImage = capteurs;
            })
        }
    }
}
