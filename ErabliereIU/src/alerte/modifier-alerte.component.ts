import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { UntypedFormGroup, UntypedFormBuilder, ReactiveFormsModule } from "@angular/forms";
import { Subject } from "rxjs";
import { AlerteCapteur } from "src/model/alerteCapteur";
import { convertTenthToNormale, divideByTen, divideNByTen } from "src/core/calculator.service";
import { EinputComponent } from "../formsComponents/einput.component";
import { NgIf } from "@angular/common";

@Component({
    selector: 'modifier-alerte-modal',
    templateUrl: 'modifier-alerte.component.html',
    standalone: true,
    imports: [NgIf, ReactiveFormsModule, EinputComponent]
})
export class ModifierAlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.alerteForm = this.fb.group({
            nom: '',
            destinataireCourriel: '',
            destinataireSMS: '',
            temperatureMin: '',
            temperatureMax: '',
            vacciumMin: '',
            vacciumMax: '',
            niveauBassinMin: '',
            niveauBassinMax: ''
        });
        this.alerteCapteurForm = this.fb.group({
            nom: '',
            destinataireCourriel: '',
            destinataireSMS: '',
            min: '',
            max: ''
        });
    }
    
    ngOnInit(): void {
        let alerte = this.alerte;

        if (alerte != undefined) {
            this.alerteForm.setValue({
                nom: alerte.nom,
                destinataireCourriel: alerte.envoyerA,
                destinataireSMS: alerte.texterA,
                temperatureMin: divideByTen(alerte.temperatureThresholdHight),
                temperatureMax: divideByTen(alerte.temperatureThresholdLow),
                vacciumMin: divideByTen(alerte.vacciumThresholdHight),
                vacciumMax: divideByTen(alerte.vacciumThresholdLow),
                niveauBassinMin: alerte.niveauBassinThresholdHight,
                niveauBassinMax: alerte.niveauBassinThresholdLow
            });
        }

        let alerteCapteur = this.alerteCapteur;

        if (alerteCapteur != undefined) {
            this.alerteCapteurForm.setValue({
                nom: alerteCapteur.nom,
                destinataireCourriel: alerteCapteur.envoyerA,
                destinataireSMS: alerteCapteur.texterA,
                min: divideNByTen(alerteCapteur.minVaue),
                max: divideNByTen(alerteCapteur.maxValue)
            });
        }
    }

    @Input() alerte?:Alerte;
    @Input() alerteCapteur?:AlerteCapteur
    @Input() idErabliereSelectionee:any

    @Input() displayEditFormSubject = new Subject<string>();
    @Input() alerteEditFormSubject = new Subject<Alerte>();
    @Input() alerteCapteurEditFormSubject = new Subject<AlerteCapteur>();

    alerteForm: UntypedFormGroup;
    alerteCapteurForm: UntypedFormGroup;

    @Input() editAlerte: boolean = false;
    @Input() editAlerteCapteur: boolean = false;

    generalError?: string;

    onSubmit() {

    }

    onButtonAnnuleClick() {
        this.displayEditFormSubject.next("");
    }

    onButtonModifierClick() {
        let alerte = new Alerte();

        alerte.id = this.alerte?.id;
        alerte.idErabliere = this.idErabliereSelectionee;
        alerte.nom = this.alerteForm.controls['nom'].value;
        alerte.envoyerA = this.alerteForm.controls['destinataireCourriel'].value;
        alerte.texterA = this.alerteForm.controls['destinataireSMS'].value;
        alerte.temperatureThresholdLow = convertTenthToNormale(this.alerteForm.controls['temperatureMax'].value);
        alerte.temperatureThresholdHight = convertTenthToNormale(this.alerteForm.controls['temperatureMin'].value);
        alerte.vacciumThresholdLow = convertTenthToNormale(this.alerteForm.controls['vacciumMax'].value);
        alerte.vacciumThresholdHight = convertTenthToNormale(this.alerteForm.controls['vacciumMin'].value);
        alerte.niveauBassinThresholdLow = this.alerteForm.controls['niveauBassinMax'].value;
        alerte.niveauBassinThresholdHight = this.alerteForm.controls['niveauBassinMin'].value;
        alerte.isEnable = this.alerte?.isEnable;
        this._api.putAlerte(this.idErabliereSelectionee, alerte)
                 .then(r => {
                     this.displayEditFormSubject.next("");
                     this.alerteEditFormSubject.next(r);
                 })
                 .catch(e => {
                     this.generalError = "Erreur lors de la modification de l'alerte";
                });
    }

    onButtonModifierAlerteCapteurClick() {
        const alerte = new AlerteCapteur();

        alerte.id = this.alerteCapteur?.id;
        alerte.idCapteur = this.alerteCapteur?.idCapteur;
        alerte.nom = this.alerteCapteurForm.controls['nom'].value;
        alerte.envoyerA = this.alerteCapteurForm.controls['destinataireCourriel'].value;
        alerte.texterA = this.alerteCapteurForm.controls['destinataireSMS'].value;
        alerte.isEnable = this.alerteCapteur?.isEnable;
        var minInForm = this.alerteCapteurForm.controls['min'].value;
        if (minInForm != null && (minInForm !== "" || minInForm === 0)) {
            console.log('parseMin')
            alerte.minVaue = parseInt(convertTenthToNormale(minInForm.toString()));
        } else {
            console.log('min to undefined')
            alerte.minVaue = undefined;
        }
        var maxInForm = this.alerteCapteurForm.controls['max'].value;
        if (maxInForm != null && (maxInForm !== "" || maxInForm === 0)) {
            alerte.maxValue = parseInt(convertTenthToNormale(maxInForm.toString()));
        } else {
            alerte.maxValue = undefined;
        }
        this._api.putAlerteCapteur(alerte.idCapteur, alerte)
                 .then(r => {
                     this.displayEditFormSubject.next("");
                     this.alerteCapteurEditFormSubject.next(r);
                 })
                 .catch(e => {
                    this.generalError = "Erreur lors de la modification de l'alerte";
                 });
    }
}