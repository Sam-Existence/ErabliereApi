import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { FormGroup, FormBuilder } from "@angular/forms";
import { Subject } from "rxjs";
import { AlerteCapteur } from "src/model/alerteCapteur";

@Component({
    selector: 'modifier-alerte-modal',
    templateUrl: 'modifier-alerte.component.html'
})
export class ModifierAlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: FormBuilder) {
        this.alerteForm = this.fb.group({
            id: '',
            destinataire: '',
            temperatureMin: '',
            temperatureMax: '',
            vacciumMin: '',
            vacciumMax: '',
            niveauBassinMin: '',
            niveauBassinMax: ''
        });
        this.alerteCapteurForm = this.fb.group({
            id: '',
            idCapteur: '',
            destinataire: '',
            min: '',
            max: ''
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
                niveauBassinMax: alerte.niveauBassinThresholdLow
            });
        }

        let alerteCapteur = this.alerteCapteur;

        if (alerteCapteur != undefined) {
            this.alerteCapteurForm.setValue({
                id: alerteCapteur.id,
                idCapteur: alerteCapteur.idCapteur,
                destinataire: alerteCapteur.envoyerA,
                min: alerteCapteur.minVaue,
                max: alerteCapteur.maxValue
            });
        }
    }

    @Input() alerte?:Alerte;
    @Input() alerteCapteur?:AlerteCapteur
    @Input() idErabliereSelectionee:any

    @Input() displayEditFormSubject = new Subject<string>();
    @Input() alerteEditFormSubject = new Subject<Alerte>();
    @Input() alerteCapteurEditFormSubject = new Subject<AlerteCapteur>();

    alerteForm: FormGroup;
    alerteCapteurForm: FormGroup;

    @Input() editAlerte: boolean = false;
    @Input() editAlerteCapteur: boolean = false;

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
        this._api.putAlerte(this.idErabliereSelectionee, alerte)
                 .then(r => {
                     this.displayEditFormSubject.next("");
                     this.alerteEditFormSubject.next(r);
                 });
    }

    onButtonModifierAlerteCapteurClick() {
        let alerte = new AlerteCapteur();

        alerte.id = this.alerteForm.controls['id'].value;
        alerte.idCapteur = this.alerteForm.controls['idCapteur'].value;
        alerte.envoyerA = this.alerteForm.controls['destinataire'].value;
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
        this._api.putAlerteCapteur(this.idErabliereSelectionee, alerte)
                 .then(r => {
                     this.displayEditFormSubject.next("");
                     this.alerteCapteurEditFormSubject.next(r);
                 });
    }
}