import { Component, Input, OnInit } from "@angular/core";
import { Subject } from "rxjs";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { Alerte } from "src/model/alerte";

@Component({
    selector: 'alerte-page',
    template: `
        <h3>Alertes</h3>
        <ajouter-alerte-modal *ngIf="!displayEditForm"
            [idErabliereSelectionee]="idErabliereSelectionee"
            [alertes]="alertes">
        </ajouter-alerte-modal>
        <modifier-alerte-modal *ngIf="displayEditForm"
            [idErabliereSelectionee]="idErabliereSelectionee"
            [displayEditFormSubject]="displayEditFormSubject"
            [alerteEditFormSubject]="alerteEditFormSubject"
            [alerte]="alerteEditForm">
        </modifier-alerte-modal>
        <div>
            <p *ngIf="alertes != null && alertes.length == 0">Aucune alerte de configuré</p>
            <table *ngIf="alertes != null && alertes.length > 0" class="table">
                <thead>
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            Envoyer à
                        </th>
                        <th>
                            Temperature min.
                        </th>
                        <th>
                            Temperature max.
                        </th>
                        <th>
                            Vaccium min.
                        </th>
                        <th>
                            Vaccium max.
                        </th>
                        <th>
                            Niveau bassin min.
                        </th>
                        <th>
                            Niveau bassin max.
                        </th>
                        <th></th>
                    <tr>
                </thead>
                <tbody>
                    <tr *ngFor="let alerte of alertes">
                        <td>
                            {{alerte.id}}
                        </td>
                        <td>
                            {{alerte.envoyerA}}
                                
                        </td>
                        <td>
                            {{alerte.temperatureThresholdHight}}
                        </td>
                        <td>
                            {{alerte.temperatureThresholdLow}}
                        </td>
                        <td>
                            {{alerte.vacciumThresholdHight}}
                        </td>
                        <td>
                            {{alerte.vacciumThresholdLow}}
                        </td>
                        <td>
                            {{alerte.niveauBassinThresholdHight}}
                        </td>
                        <td>
                            {{alerte.niveauBassinThresholdLow}}
                        </td>
                        <td>
                            <button (click)="onButtonModifierClick(alerte.id)">modifier</button>
                            <button (click)="onButtonDeleteClick(alerte.id)">supprimer</button>
                        </td>
                    <tr>
                </tbody>
            </table>
        </div>
    `
})
export class AlerteComponent implements OnInit {
    constructor(private _api: ErabliereApi) { }

    ngOnInit(): void {
        this.displayEditFormSubject.subscribe(b => {
             this.displayEditForm = b.valueOf();
        });

        this.alerteEditFormSubject.subscribe(b => {
            if (this.alertes != undefined) {
                var i = this.alertes.findIndex(a => a.id == this.alerteEditForm?.id);

                this.alertes[i] = b;
            }
        });
    }

    @Input() alertes?: Array<Alerte>;

    @Input() idErabliereSelectionee:any

    displayEditFormSubject = new Subject<Boolean>();
    displayEditFormObservable = this.displayEditFormSubject.asObservable();
    displayEditForm: boolean = false;

    alerteEditFormSubject = new Subject<Alerte>();
    alerteEditFormObservable = this.displayEditFormSubject.asObservable();
    alerteEditForm?: Alerte;

    onButtonModifierClick(alerteId:any) {
        this.alerteEditForm = this.alertes?.find(a => a.id == alerteId);
        this.alerteEditFormSubject.next(this.alerteEditForm);
        this.displayEditFormSubject.next(true);
    }

    onButtonDeleteClick(alerteId:any) {
        if (confirm("Voulez-vous vraiment supprimer l'alerte " + alerteId + " ? ")) {
            this._api.deleteAlerte(this.idErabliereSelectionee, alerteId)
                     .then(a => {
                        this._api.getAlertes(this.idErabliereSelectionee)
                                 .then(a => {
                                     this.alertes = a;
                                 });
                     });
        }
    }
}