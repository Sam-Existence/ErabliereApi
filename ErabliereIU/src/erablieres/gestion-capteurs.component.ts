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
    constructor(private readonly erabliereApi: ErabliereApi) {
    }

    @Input() idErabliere?: any;
    @Input() capteurs: Capteur[] = [];
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