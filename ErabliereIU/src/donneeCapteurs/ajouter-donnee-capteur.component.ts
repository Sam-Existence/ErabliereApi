import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { DonneeCapteur } from "src/model/donneeCapteur";

@Component({
    selector: 'ajouter-donnee-capteur',
    template: `
        <div class="border-top">
            <h3>Ajouter une donnée</h3>
            <div class="form">
                <div class="form-group">
                    <label for="valeur">Valeur</label>
                    <input type="number" class="form-control" id="valeur" placeholder="Valeur">
                </div>
                <div class="form-group">
                    <label for="date">Date</label>
                    <input type="date" class="form-control" id="date" placeholder="Date">
                </div>
                <button type="button" class="btn btn-primary" (click)="ajouterDonnee()">Ajouter</button>
                <button type="button" class="btn btn-secondary" (click)="annuler()">Annuler</button>
            </div>
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
    valeur?: number;
    date?: string;

    constructor(private api: ErabliereApi) { }

    ngOnInit(): void {
    }

    ajouterDonnee() {
        var donneeCapteur = new DonneeCapteur();
        donneeCapteur.valeur = this.valeur;
        donneeCapteur.d = this.date;
        donneeCapteur.id = this.idCapteur;

        console.log(JSON.stringify(donneeCapteur));

        this.api.postDonneeCapteur(this.idCapteur, donneeCapteur).then(() => {
            console.log("Donnée ajoutée");
        });
    }

    annuler() {

    }
}