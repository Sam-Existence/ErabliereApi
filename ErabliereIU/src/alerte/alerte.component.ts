import { Component, Input, OnInit } from "@angular/core";
import { AuthorisationService } from "src/authorisation/authorisation-service.component";
import { environment } from "src/environments/environment";

@Component({
    selector: 'alerte-page',
    template: `
        <h3>Alerte</h3>
        <div>
            <p *ngIf="access_token == null">Vous devez être authentifié pour visionner les alertes.</p>
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

    @Input() access_token: any;
    @Input() alertes?: Array<any>;

}