import {AfterViewChecked, Component, EventEmitter, Input, OnChanges, OnInit, Output} from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { PutCapteur } from "src/model/putCapteur";
import { InputErrorComponent } from "../formsComponents/input-error.component";
import {
    ReactiveFormsModule,
    FormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    FormControl,
    Validators
} from "@angular/forms";

@Component({
    selector: 'ajouter-capteur',
    templateUrl: 'ajouter-capteur.component.html',
    standalone: true,
    imports: [ReactiveFormsModule, FormsModule, InputErrorComponent]
})
export class AjouterCapteurComponent implements AfterViewChecked {
    ajoutCapteurForm: UntypedFormGroup;
    @Input() idErabliere?: string;
    @Output() hideAjouterCapteur = new EventEmitter();
    @Output() shouldReloadCapteurs = new EventEmitter();

    capteur: PutCapteur = new PutCapteur();

    errorObj: any;
    generalError: string | undefined;

    constructor(private readonly erabliereApi: ErabliereApi,
                private formBuilder: UntypedFormBuilder) {
        this.ajoutCapteurForm = this.formBuilder.group({
            nom: new FormControl(
                '',
                {
                    validators: [Validators.required, Validators.maxLength(50)],
                    updateOn: 'blur'
                }
            ),
            symbole: new FormControl(
                '',
                {
                    validators: [Validators.maxLength(5)],
                    updateOn: 'blur'
                }
            ),
            affichageDashboard: new FormControl(
                false
            ),
            saisieManuelle: new FormControl(
                false
            )
        })
    }

    ajouterCapteur() {
        this.capteur.idErabliere = this.idErabliere;
        this.capteur.nom = this.ajoutCapteurForm.controls['nom'].value;
        this.capteur.symbole = this.ajoutCapteurForm.controls['symbole'].value;
        this.capteur.afficherCapteurDashboard = this.ajoutCapteurForm.controls['affichageDashboard'].value;
        this.capteur.ajouterDonneeDepuisInterface = this.ajoutCapteurForm.controls['saisieManuelle'].value;
        this.erabliereApi.postCapteur(this.idErabliere, this.capteur).then(() => {
            this.shouldReloadCapteurs.emit();
            this.ajoutCapteurForm.reset();
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

    ngAfterViewChecked() {
        console.log(this.ajoutCapteurForm.controls['nom'].errors);
    }
}

