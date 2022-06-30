// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Erabliere } from 'src/model/erabliere';

@Component({
    selector: 'ajouter-erabliere',
    templateUrl: './ajouter-erabliere.component.html',
})
export class AjouterErabliereComponent implements OnInit {
    getDefaultErabliere() {
        let e = new Erabliere();
        e.afficherSectionBaril = true;
        e.afficherSectionDompeux = true;
        e.afficherTrioDonnees = true;
        e.ipRule = "-";
        return e;
    }

    erabliere: Erabliere = this.getDefaultErabliere();
    plusdOptions: boolean = false;
    plusOptionsButtonText: string = "Plus d'options";
    @Output() shouldReloadErablieres = new EventEmitter();

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
    }

    ajouterErabliere() {
        if (this.erabliere != undefined) {
            this._api.postErabliere(this.erabliere).then(() => {
                this.erabliere = this.getDefaultErabliere();
                this.shouldReloadErablieres.emit();
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