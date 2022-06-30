// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'ajouter-erabliere',
    templateUrl: './ajouter-erabliere.component.html',
})
export class AjouterErabliereComponent implements OnInit {
    erabliere?: Erabliere;
    plusdOptions: boolean = false;
    plusOptionsButtonText: string = "Plus d'options";

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
    }

    ajouterErabliere() {
        if (this.erabliere != undefined) {
            this._api.postErabliere(this.erabliere).then(() => {
                this.erabliere = undefined;
            });
        }
    }

    afficherPlusOptions() {
        this.plusdOptions = !this.plusdOptions;
        if (this.plusdOptions) {
            this.plusOptionsButtonText = "Moins d'options";
        } else {
            this.plusOptionsButtonText = "Plus d'options";
        }
    }
}