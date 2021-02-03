import { Component } from '@angular/core';

@Component({
    selector: 'menu-erabliere',
    template: `
        <div>
            <ul>
                <li *ngFor="let erabliere of erablieres">{{erabliere.nom}}</li>
            </ul>
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