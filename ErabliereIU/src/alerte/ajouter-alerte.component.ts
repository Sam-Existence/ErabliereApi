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

    typeAlerteList = [
        { id: 1, name: 'Alerte trio de donnée' },
        { id: 2, name: 'Alerte de capteur' }
    ];

    typeAlerteSelectListForm = new UntypedFormGroup({
        state: new UntypedFormControl(this.typeAlerteList[0].id)
    });
    
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
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
    
    display:string = "";

    alerte:Alerte = new Alerte();
    alerteCapteur:AlerteCapteur = new AlerteCapteur();

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
        this.display = "alerte";
    }

    onButtonAnnuleClick() {
        this.display = "";
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
                         this.display = "";
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
                         this.display = "";
                         this.alertesCapteur?.push(r);
                         console.log(this.alertesCapteur);
                     });
        }
        else {
            console.log("this.alerteCapteur is undefined");
        }
    }
}