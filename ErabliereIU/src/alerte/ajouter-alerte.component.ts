import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { UntypedFormGroup, UntypedFormBuilder, UntypedFormControl } from "@angular/forms";
import { AlerteCapteur } from "src/model/alerteCapteur";
import { Capteur } from "src/model/capteur";

@Component({
    selector: 'ajouter-alerte-modal',
    templateUrl: 'ajouter-alerte.component.html'
})
export class AjouterAlerteComponent implements OnInit {
    typeAlerteSelectListForm: UntypedFormGroup
    alerte:Alerte
    alerteCapteur:AlerteCapteur
    display:boolean
    generalError?: string
    
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.alerte = new Alerte();
        this.alerteCapteur = new AlerteCapteur();
        this.typeAlerteSelectListForm = new UntypedFormGroup({
            state: new UntypedFormControl(1) // 1 is the value of the first option in the select list
        });
        this.display = false;
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
    
    

    @Input() alertes?: Array<Alerte>;
    @Input() alertesCapteur?: Array<AlerteCapteur>;

    @Input() idErabliereSelectionee:any
    capteurs: Array<Capteur> = [];

    alerteForm: UntypedFormGroup;
    alerteCapteurForm: UntypedFormGroup;

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
                         r.emails = r.envoyerA.split(";");
                         this.alertes?.push(r);
                     })
                     .catch(e => {
                        this.generalError = "Erreur lors de la modification de l'alerte";
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
            if (this.alerteCapteurForm.controls['min'].value != "") {
                this.alerteCapteur.minVaue = parseInt(this.alerteCapteurForm.controls['min'].value);
            } else {
                this.alerteCapteur.minVaue = undefined;
            }
            if (this.alerteCapteurForm.controls['max'].value != "") {
                this.alerteCapteur.maxValue = parseInt(this.alerteCapteurForm.controls['max'].value);
            } else {
                this.alerteCapteur.maxValue = undefined;
            }
            this._api.postAlerteCapteur(this.alerteCapteur.idCapteur, this.alerteCapteur)
                     .then(r => {
                         this.display = false;
                         r.emails = r?.envoyerA?.split(";");
                         this.alertesCapteur?.push(r);
                     })
                     .catch(e => {
                        this.generalError = "Erreur lors de la modification de l'alerte";
                     });
        }
        else {
            console.log("this.alerteCapteur is undefined");
        }
    }
}