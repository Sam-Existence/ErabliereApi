import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { UntypedFormGroup, UntypedFormBuilder } from "@angular/forms";
import { Subject } from "rxjs";
import { AlerteCapteur } from "src/model/alerteCapteur";
import { convertTenthToNormale, divideByTen, divideNByTen } from "src/core/calculator.service";

@Component({
    selector: 'modifier-alerte-modal',
    templateUrl: 'modifier-alerte.component.html'
})
export class ModifierAlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.alerteForm = this.fb.group({
            id: '',
            nom: '',
            destinataire: '',
            temperatureMin: '',
            temperatureMax: '',
            vacciumMin: '',
            vacciumMax: '',
            niveauBassinMin: '',
            niveauBassinMax: '',
            isEnable: ''
        });
        this.alerteCapteurForm = this.fb.group({
            destinataire: '',
            nom: '',
            min: '',
            max: '',
            isEnable: ''
        });
    }
    
    ngOnInit(): void {
        let alerte = this.alerte;

        if (alerte != undefined) {
            this.alerteForm.setValue({
                id: alerte.id,
                nom: alerte.nom,
                destinataire: alerte.envoyerA,
                temperatureMin: divideByTen(alerte.temperatureThresholdHight),
                temperatureMax: divideByTen(alerte.temperatureThresholdLow),
                vacciumMin: divideByTen(alerte.vacciumThresholdHight),
                vacciumMax: divideByTen(alerte.vacciumThresholdLow),
                niveauBassinMin: alerte.niveauBassinThresholdHight,
                niveauBassinMax: alerte.niveauBassinThresholdLow,
                isEnable: alerte.isEnable
            });
        }

        let alerteCapteur = this.alerteCapteur;

        if (alerteCapteur != undefined) {
            this.alerteCapteurForm.setValue({
                destinataire: alerteCapteur.envoyerA,
                nom: alerteCapteur.nom,
                min: divideNByTen(alerteCapteur.minVaue),
                max: divideNByTen(alerteCapteur.maxValue),
                isEnable: alerteCapteur.isEnable
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

        alerte.id = this.alerteForm.controls['id'].value;
        alerte.idErabliere = this.idErabliereSelectionee;
        alerte.nom = this.alerteForm.controls['nom'].value;
        alerte.envoyerA = this.alerteForm.controls['destinataire'].value;
        alerte.temperatureThresholdLow = convertTenthToNormale(this.alerteForm.controls['temperatureMax'].value);
        alerte.temperatureThresholdHight = convertTenthToNormale(this.alerteForm.controls['temperatureMin'].value);
        alerte.vacciumThresholdLow = convertTenthToNormale(this.alerteForm.controls['vacciumMax'].value);
        alerte.vacciumThresholdHight = convertTenthToNormale(this.alerteForm.controls['vacciumMin'].value);
        alerte.niveauBassinThresholdLow = this.alerteForm.controls['niveauBassinMax'].value;
        alerte.niveauBassinThresholdHight = this.alerteForm.controls['niveauBassinMin'].value;
        alerte.isEnable = this.alerteForm.controls['isEnable'].value;
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
        alerte.envoyerA = this.alerteCapteurForm.controls['destinataire'].value;
        alerte.nom = this.alerteCapteurForm.controls['nom'].value;
        alerte.isEnable = this.alerteCapteurForm.controls['isEnable'].value;
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