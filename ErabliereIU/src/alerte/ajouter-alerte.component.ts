import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { FormGroup, FormBuilder } from "@angular/forms";

@Component({
    selector: 'ajouter-alerte-modal',
    templateUrl: 'ajouter-alerte.component.html'
})
export class AjouterAlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: FormBuilder) 
    {
        this.alerteForm = this.fb.group({});
    }
    
    ngOnInit(): void {
        this.initializeForm();
    }

    initializeForm() {
        this.alerteForm = this.fb.group({
            destinataire: '',
            temperatureMin: '',
            temperatureMax: '',
            vacciumMin: '',
            vacciumMax: '',
            niveauBassinMin: '',
            niveauBassinMax: ''
        });
    }
    
    display:boolean = false;

    alerte:Alerte = new Alerte();

    @Input() alertes?: Array<Alerte>;

    @Input() idErabliereSelectionee:any

    alerteForm: FormGroup;

    onSubmit() {

    }

    onButtonAjouterClick() {
        this.display = true;
    }

    onButtonAnnuleClick() {
        this.display = false;
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
            console.log(JSON.stringify(this.alerte));
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
}