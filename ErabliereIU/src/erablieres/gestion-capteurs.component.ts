import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";

@Component({
    selector: 'gestion-capteurs',
    templateUrl: 'gestion-capteurs.component.html'
  })
export class GestionCapteursComponent implements OnInit {

    @Input() idErabliere?: string;

    constructor(private readonly erabliereApi: ErabliereApi) {
    }

    capteurs: Capteur[] = [];
    afficherSectionAjouterCapteur: boolean = false;

    async ngOnInit(): Promise<void> {
        this.capteurs = await this.erabliereApi.getCapteurs(this.idErabliere);
    }

    showAjouterCapteur() {
        this.afficherSectionAjouterCapteur = true;
    }

    hideAjouterCapteur() {
        this.afficherSectionAjouterCapteur = false;
    }
}