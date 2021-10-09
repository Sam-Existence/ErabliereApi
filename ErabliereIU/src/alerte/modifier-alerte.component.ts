import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";
import { FormGroup, FormBuilder } from "@angular/forms";

@Component({
    selector: 'modifier-alerte-modal',
    templateUrl: 'modifier-alerte.component.html'
})
export class ModifierAlerteComponent implements OnInit {
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
    
    @Input() display:boolean = false;

    @Input() alerte?:Alerte;

    @Input() idErabliereSelectionee:any

    alerteForm: FormGroup;

    onSubmit() {

    }

    onButtonAnnuleClick() {
        
    }

    onButtonModifierClick() {
        if (this.alerte != undefined) {
            this.alerte.idErabliere = this.idErabliereSelectionee;
            this.alerte.envoyerA = this.alerteForm.controls['destinataire'].value;
            this.alerte.temperatureThresholdLow = this.alerteForm.controls['temperatureMin'].value;
            this.alerte.temperatureThresholdHight = this.alerteForm.controls['temperatureMax'].value;
            this.alerte.vacciumThresholdLow = this.alerteForm.controls['vacciumMin'].value;
            this.alerte.vacciumThresholdHight = this.alerteForm.controls['vacciumMax'].value;
            this.alerte.niveauBassinThresholdLow = this.alerteForm.controls['niveauBassinMin'].value;
            this.alerte.niveauBassinThresholdHight = this.alerteForm.controls['niveauBassinMax'].value;
            this._api.putAlerte(this.idErabliereSelectionee, this.alerte)
                     .then(r => {
                         this.display = false;
                     });
        }
        else {
            console.log("this.alerte is undefined");
        }
    }
}