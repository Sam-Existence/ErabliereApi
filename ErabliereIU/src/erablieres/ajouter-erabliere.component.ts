// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component'

@Component({
    selector: 'ajouter-erabliere',
    templateUrl: './ajouter-erabliere.component.html',
})
export class AjouterErabliereComponent implements OnInit {
    @ViewChild(ErabliereFormComponent) erabliereForm?: ErabliereFormComponent;
    modalTitle: string = "Ajouter une érablière";
    @Output() shouldReloadErablieres = new EventEmitter();

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
    }

    ajouterErabliere() {
        if (this.erabliereForm != undefined) {
            if (this.erabliereForm.erabliere != undefined) {
                let erabliere = this.erabliereForm.erabliere;

                this._api.postErabliere(erabliere).then(() => {
                    if (this.erabliereForm != undefined) {
                        this.erabliereForm.erabliere = this.erabliereForm.getDefaultErabliere();
                    }
                    this.shouldReloadErablieres.emit();
                });
            }
        }
    }
}