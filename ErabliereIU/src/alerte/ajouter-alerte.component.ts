import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { FormGroup, FormBuilder } from "@angular/forms";
import { AlerteCapteur } from "src/model/alerteCapteur";
import { Capteur } from "src/model/capteur";

@Component({
    selector: 'ajouter-alerte-modal',
    templateUrl: 'ajouter-alerte.component.html'
})
export class AjouterAlerteComponent implements OnInit {
    
    constructor(private _api: ErabliereApi, private fb: FormBuilder) {
        this.alerteForm = this.fb.group({});
        this.alerteCapteurForm = this.fb.group({});
    }
    
    ngOnInit(): void {
        this.initializeForms();
    }

    initializeForms() {
        this.alerteForm = this.fb.group({
            destinataire: '',
            temperatureMin: '',
            temperatureMax: '',
            vacciumMin: '',
            vacciumMax: '',
            niveauBassinMin: '',
            niveauBassinMax: ''
        });
        this.alerteCapteurForm = this.fb.group({
            destinataire: '',
            min: '',
            max: '',
            idCapteur: ''
        });
    }
    
    display:boolean = false;

    alerte:Alerte = new Alerte();
    alerteCapteur:AlerteCapteur = new AlerteCapteur();

    @Input() alertes?: Array<Alerte>;
    @Input() alertesCapteur?: Array<AlerteCapteur>;

    @Input() idErabliereSelectionee:any
    capteurs: Array<Capteur> = [];

    alerteForm: FormGroup;
    alerteCapteurForm: FormGroup;

    typeAlerte:number = 1;

    onSubmit() {

    }

    onButtonAjouterClick() {
        this.display = true;
    }

    onButtonAnnuleClick() {
        this.display = false;
    }

    onChangeAlerteType(event:any) {
        this.typeAlerte = event.target.value;

        if (this.typeAlerte == 2) {
            this._api.getCapteurs(this.idErabliereSelectionee).then(r => {
                this.capteurs = r;
            });
        }
    }

    onButtonCreerClick() {
        if (this.alerte != undefined) {
            this.alerte.idErabliere = this.idErabliereSelectionee;
            this.alerte.envoyerA = this.alerteForm.controls['destinataire'].value;
            this.alerte.temperatureThresholdLow = this.alerteForm.controls['temperatureMax'].value;
            this.alerte.temperatureThresholdHight = this.alerteForm.controls['temperatureMin'].value;
            this.alerte.vacciumThresholdLow = this.alerteForm.controls['vacciumMax'].value;
            this.alerte.vacciumThresholdHight = this.alerteForm.controls['vacciumMin'].value;
            this.alerte.niveauBassinThresholdLow = this.alerteForm.controls['niveauBassinMax'].value;
            this.alerte.niveauBassinThresholdHight = this.alerteForm.controls['niveauBassinMin'].value;
            this._api.postAlerte(this.idErabliereSelectionee, this.alerte)
                     .then(r => {
                         this.display = false;
                         this.alertes?.push(r);
                     });
        }
        else {
            console.log("this.alerte is undefined");
        }
    }

    onButtonCreerAlerteCapteurClick() {
        if (this.alerteCapteur != undefined) {
            this.alerteCapteur.idCapteur = this.alerteCapteurForm.controls['idCapteur'].value;
            this.alerteCapteur.envoyerA = this.alerteCapteurForm.controls['destinataire'].value;
            this.alerteCapteur.minValue = this.alerteCapteurForm.controls['min'].value;
            this.alerteCapteur.maxValue = this.alerteCapteurForm.controls['max'].value;
            this._api.postAlerteCapteur(this.alerteCapteur.idCapteur, this.alerteCapteur)
                     .then(r => {
                         this.display = false;
                         this.alertesCapteur?.push(r);
                     });
        }
        else {
            console.log("this.alerteCapteur is undefined");
        }
    }
}