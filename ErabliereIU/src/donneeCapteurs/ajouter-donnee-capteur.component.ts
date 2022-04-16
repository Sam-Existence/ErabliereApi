import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { PostDonneeCapteur } from "src/model/donneeCapteur";

@Component({
    selector: 'ajouter-donnee-capteur',
    template: `
        <button *ngIf="!display" class="btn btn-primary" (click)="afficherForm()">Ajouter</button>
        <div *ngIf="display" class="border-top">
            <h3>Ajouter une donnée</h3>
            <form [formGroup]="donneeCapteurForm">
                <div class="form-group">
                    <label for="valeur">Valeur</label>
                    <input type="number" class="form-control" id="valeur" name="valeur" placeholder="Valeur" formControlName="valeur">
                </div>
                <div class="form-group">
                    <label for="date">Date</label>
                    <input type="datetime-local" class="form-control" id="date" name="date" placeholder="Date" formControlName="date">
                </div>
                <button type="button" class="btn btn-primary" (click)="ajouterDonnee()">Ajouter</button>
                <button type="button" class="btn btn-secondary" (click)="annuler()">Annuler</button>
            </form>
        </div>
    `,
    styles: [`
        .border-top {
            border-top: 1px solid #ccc;
        }
    `]
})
export class AjouterDonneeCapteurComponent implements OnInit {
    @Input() idCapteur: any;
    donneeCapteurForm: FormGroup;
    valeurValidationError: string = '';
    dateValidationError: string = '';
    display: boolean = false;

    @Output() needToUpdate = new EventEmitter();

    constructor(private api: ErabliereApi, private fb: FormBuilder) {
        this.donneeCapteurForm = this.fb.group({});
     }

    ngOnInit(): void {
        this.donneeCapteurForm = this.fb.group({
            valeur: '',
            date: ''
        });
    }

    onSubmit() {

    }

    ajouterDonnee() {
        var donneeCapteur = new PostDonneeCapteur();
        var validationError = false;
        try {
            donneeCapteur.v = parseInt(this.donneeCapteurForm.controls['valeur'].value);
        } catch (error) {
            validationError = true;
        }
        try {
            donneeCapteur.d = new Date(this.donneeCapteurForm.controls['date'].value).toISOString();
        } catch (error) {
            validationError = true;
        }
        
        if (validationError == false) {
            donneeCapteur.idCapteur = this.idCapteur;

            this.api.postDonneeCapteur(this.idCapteur, donneeCapteur).then(() => {
                this.donneeCapteurForm.reset();
                this.needToUpdate.emit();
            });
        }
    }

    annuler() {
        this.donneeCapteurForm.reset();
        this.display = false;
    }

    afficherForm() {
        this.display = true;
    }
}