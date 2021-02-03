import { Component } from '@angular/core';

@Component({
    selector: 'barils-panel',
    template: `
        <div class="border-top">
            <h3>Barils</h3>
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            Numéro
                        </th>
                    <tr>
                </thead>
                <tbody>
                    <tr *ngFor="let baril of barils">
                        <td>
                            {{baril.id}}
                        </td>
                    <tr>
                </tbody>
            </table>
        </div>
    `
})
export class BarilsComponent {
    barils:any;
    constructor(){
        fetch("http://localhost:5000/erablieres/0/baril?dd=2021-03-15&df=2021-04-15T00:05:00")
            .then(e => e.json())
            .then(d => this.barils = d);
    }
}