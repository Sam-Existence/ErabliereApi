import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { UntypedFormGroup, UntypedFormBuilder } from "@angular/forms";
import { Subject } from "rxjs";
import { AlerteCapteur } from "src/model/alerteCapteur";

@Component({
    selector: 'modifier-alerte-modal',
    templateUrl: 'modifier-alerte.component.html'
})
export class ModifierAlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.alerteForm = this.fb.group({
            id: '',
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
                destinataire: alerte.envoyerA,
                temperatureMin: alerte.temperatureThresholdHight,
                temperatureMax: alerte.temperatureThresholdLow,
                vacciumMin: alerte.vacciumThresholdHight,
                vacciumMax: alerte.vacciumThresholdLow,
                niveauBassinMin: alerte.niveauBassinThresholdHight,
                niveauBassinMax: alerte.niveauBassinThresholdLow,
                isEnable: alerte.isEnable
            });
        }

        let alerteCapteur = this.alerteCapteur;

        if (alerteCapteur != undefined) {
            this.alerteCapteurForm.setValue({
                destinataire: alerteCapteur.envoyerA,
                min: alerteCapteur.minVaue,
                max: alerteCapteur.maxValue,
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
        alerte.envoyerA = this.alerteForm.controls['destinataire'].value;
        alerte.temperatureThresholdLow = this.alerteForm.controls['temperatureMax'].value;
        alerte.temperatureThresholdHight = this.alerteForm.controls['temperatureMin'].value;
        alerte.vacciumThresholdLow = this.alerteForm.controls['vacciumMax'].value;
        alerte.vacciumThresholdHight = this.alerteForm.controls['vacciumMin'].value;
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
        let alerte = new AlerteCapteur();

        alerte.id = this.alerteCapteur?.id;
        alerte.idCapteur = this.alerteCapteur?.idCapteur;
        alerte.envoyerA = this.alerteCapteurForm.controls['destinataire'].value;
        alerte.isEnable = this.alerteCapteurForm.controls['isEnable'].value;
        if (this.alerteCapteurForm.controls['min'].value != "") {
            alerte.minVaue = parseInt(this.alerteCapteurForm.controls['min'].value);
        } else {
            alerte.minVaue = undefined;
        }
        if (this.alerteCapteurForm.controls['max'].value != "") {
            alerte.maxValue = parseInt(this.alerteCapteurForm.controls['max'].value);
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