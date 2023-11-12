import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { PutCapteur } from "src/model/putCapteur";
import { InputErrorComponent } from "../formsComponents/input-error.component";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";

@Component({
    selector: 'ajouter-capteur',
    templateUrl: 'ajouter-capteur.component.html',
    standalone: true,
    imports: [ReactiveFormsModule, FormsModule, InputErrorComponent]
})
export class AjouterCapteurComponent implements OnInit {

    @Input() idErabliere?: string;
    @Output() hideAjouterCapteur = new EventEmitter();
    @Output() shouldReloadCapteurs = new EventEmitter();

    capteur: PutCapteur = new PutCapteur();

    errorObj: any;
    generalError: string | undefined;

    constructor(private readonly erabliereApi: ErabliereApi) {
    }

    ngOnInit(): void {
        
    }

    ajouterCapteur() {
        console.log(this.capteur)
        this.capteur.idErabliere = this.idErabliere;
        this.capteur.afficherCapteurDashboard = parseBoolean(this.capteur.afficherCapteurDashboard);
        this.capteur.ajouterDonneeDepuisInterface = parseBoolean(this.capteur.ajouterDonneeDepuisInterface);
        this.erabliereApi.postCapteur(this.idErabliere, this.capteur).then(() => {
            this.shouldReloadCapteurs.emit();
            this.hideAjouterCapteur.emit();
        }).catch(error => {
            if (error.status == 400) {
                this.errorObj = error
                this.generalError = undefined
            } 
            else {
                this.generalError = "Une erreur est survenue lors de l'ajout du capteur. Veuillez réessayer plus tard."
            }
        });
    }

    buttonAnnuler() {
        this.hideAjouterCapteur.emit();
    }
}

function parseBoolean(afficherCapteurDashboard: boolean | string | undefined): boolean {
    if (typeof afficherCapteurDashboard === "boolean") {
        return afficherCapteurDashboard;
    }
    if (typeof afficherCapteurDashboard === "string") {
        return afficherCapteurDashboard.toLowerCase().trim() === "true";
    }
    return false;
}
