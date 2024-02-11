import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Capteur } from "src/model/capteur";
import { TableFormInputComponent } from "../formsComponents/table-form-input.component";
import { NgFor, NgIf } from "@angular/common";

@Component({
    selector: 'capteur-list',
    templateUrl: 'capteur-list.component.html',
    standalone: true,
    imports: [NgFor, TableFormInputComponent, NgIf]
})
export class CapteurListComponent {
    @Input() idErabliere?: string;
    @Input() capteurs: Capteur[] = [];

    @Output() shouldRefreshCapteurs = new EventEmitter<void>();

    displayEdits: { [id: string]: boolean } = {};
    editedCapteurs: { [id: string]: Capteur } = {};

    constructor(private readonly erabliereApi: ErabliereApi) {

    }

    resolveDisplayEdit(capteurId: string | undefined) {
        if (!capteurId) {
            return false;
        }

        return this.displayEdits[capteurId];
    }

    showModifierCapteur(capteur: Capteur) {
        if (capteur.id) {
            this.displayEdits[capteur.id] = true;
            this.editedCapteurs[capteur.id] = { ...capteur };
        }
    }

    hideModifierCapteur(capteur: Capteur) {
        if (capteur.id) {
            this.displayEdits[capteur.id] = false;
            this.editedCapteurs[capteur.id] = { ...capteur };
        }
    }

    modifierCapteur(capteur: Capteur) {
        if (!capteur.id) {
            console.error("capteur.id is undefined in modifierCapteur()");
            return;
        }
        const putCapteur = this.editedCapteurs[capteur.id];
        this.erabliereApi.putCapteur(this.idErabliere, putCapteur).then(() => {
            this.shouldRefreshCapteurs.emit();
            if (capteur.id) {
                this.displayEdits[capteur.id] = false;
            }
        }).catch(() => {
            alert('Erreur lors de la modification du capteur');
        });
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

    editAjouterDonneeDepuisInterface(capteurId: string|undefined, newValue: string|number|boolean|undefined) {
        if (!capteurId) {
            return;
        }

        this.editedCapteurs[capteurId].ajouterDonneeDepuisInterface = newValue?.toString().trim().toLowerCase() === "true";
    }

    editAfficherCapteurDashboard(capteurId: string|undefined, newValue: string|number|boolean|undefined) {
        if (!capteurId) {
            return;
        }

        this.editedCapteurs[capteurId].afficherCapteurDashboard = newValue?.toString().trim().toLowerCase() === "true";
    }

    editSymbol(capteurId: string|undefined, newValue: string|number|boolean|undefined) {
        if (!capteurId) {
            return;
        }

        this.editedCapteurs[capteurId].symbole = newValue?.toString();
    }

    editName(capteurId: string|undefined, newValue: string|number|boolean|undefined) {
        if (!capteurId) {
            return;
        }

        this.editedCapteurs[capteurId].nom = newValue?.toString();
    }
}