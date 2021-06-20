import { Component, Input, OnInit } from "@angular/core";
import { AuthorisationService } from "src/authorisation/authorisation-service";
import { environment } from "src/environments/environment";

@Component({
    selector: 'alerte-page',
    template: `
        <h3>Alerte</h3>
        <div>
            <p *ngIf="alertes != null && alertes.length == 0">Aucune alerte de configuré</p>
            <table *ngIf="alertes != null && alertes.length > 0" class="table">
                <thead>
                    <tr>
                        <th>
                            Json
                        </th>
                    <tr>
                </thead>
                <tbody>
                    <tr *ngFor="let alerte of alertes">
                        <td>
                            {{alerte}}
                        </td>
                    <tr>
                </tbody>
            </table>
        </div>
    `
})
export class AlerteComponent{
    constructor() { }

    @Input() alertes?: Array<any>;
}