import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'barils-panel',
    template: `
        <div class="border-top">
            <h3>Barils</h3>
            <h6>Id érablière {{ erabliere.id }}</h6>
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            Numéro
                        </th>
                        <th>
                            Date fermeture
                        </th>
                        <th>
                            Estimation
                        </th>
                        <th>
                            Résultat après classement
                        </th>
                    <tr>
                </thead>
                <tbody>
                    <tr *ngFor="let baril of barils">
                        <td>
                            {{baril.id}}
                        </td>
                        <td>
                            {{baril.df}}
                        </td>
                        <td>
                            {{baril.qe}}
                        </td>
                        <td>
                            {{baril.q}}
                        </td>
                    <tr>
                </tbody>
            </table>
        </div>
    `
})
export class BarilsComponent implements OnInit {
    barils:any;
    @Input() erabliere:any

    constructor() { }

    ngOnInit() {
        fetch("https://erabliereapi.freddycoder.com/erablieres/" + this.erabliere.id + "/baril")
            .then(e => e.json())
            .then(d => this.barils = d);
    }
}