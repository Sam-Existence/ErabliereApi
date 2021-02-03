import { Component } from '@angular/core';

@Component({
    selector: 'menu-erabliere',
    template: `
    <div class="row">
        <div class="col-2 border-right">
            <ul class="list-group">
                <li class="list-group-item" *ngFor="let erabliere of erablieres">{{erabliere.nom}}</li>
            </ul>
        </div>
        <div class="col-10">
            <donnees-panel></donnees-panel>
            <dompeux-panel></dompeux-panel>
            <barils-panel></barils-panel>
        </div>
    </div>
    `
})
export class ErabliereComponent {
    erablieres:any;

    constructor(){
        fetch("http://localhost:5000/erablieres")
            .then(e => e.json())
            .then(e => this.erablieres = e);
    }
}