// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component'

@Component({
    selector: 'modifier-erabliere',
    templateUrl: './modifier-erabliere.component.html',
})
export class ModifierErabliereComponent implements OnInit {
    @ViewChild(ErabliereFormComponent) erabliereForm?: ErabliereFormComponent;
    modalTitle: string = "Modifier une erabliere";
    @Output() shouldReloadErablieres = new EventEmitter();

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
    }

    modifierErabliere() {
        if (this.erabliereForm != undefined) {
            if (this.erabliereForm.erabliere != undefined) {
                let erabliere = this.erabliereForm.erabliere;

                this._api.putErabliere(erabliere).then(() => {
                    this.shouldReloadErablieres.emit();
                });
            }
        }
    }
}