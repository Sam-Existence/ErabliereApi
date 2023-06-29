// A component pour lister les capteurs

import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";

@Component({
    selector: 'capteur-list',
    templateUrl: 'capteur-list.component.html'
    })
export class CapteurListComponent implements OnInit {

    @Input() idErabliere?: string;
    @Input() capteurs: Capteur[] = [];

    @Output() shouldRefreshCapteurs = new EventEmitter<void>();

    displayEdits: { [id: string]: boolean } = {};

    constructor(private readonly erabliereApi: ErabliereApi) {

    }

    ngOnInit(): void {
    }

    resolveDisplayEdit(capteurId: string | undefined) {
        if (!capteurId) {
            return false;
        }

        return this.displayEdits[capteurId];
    }

    showModifierCapteur(capteur: Capteur) {
        if (capteur.id)
            this.displayEdits[capteur.id] = true;
    }

    hideModifierCapteur(capteur: Capteur) {
        if (capteur.id)
            this.displayEdits[capteur.id] = false;
    }

    modifierCapteur(capteur: Capteur) {
    }

    async supprimerCapteur(capteur: Capteur) {
        if (confirm('Cette action est irréversible, vous perderez tout les données associé au capteur. Voulez-vous vraiment supprimer ce capteur?')) {
            try {
                await this.erabliereApi.deleteCapteur(this.idErabliere, { id: capteur.id, idErabliere: this.idErabliere })
                this.shouldRefreshCapteurs.emit();
            }
            catch (e) {
                alert('Erreur lors de la suppression du capteur');
            }
        }
    }

    formatDateJour(date?: Date | string): string {
        if (!date) {
            return "";
        }

        if (typeof date === "string") {
            date = new Date(date);
        }
        return date.toLocaleDateString("fr-CA");
    }
}