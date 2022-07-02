// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component'
import { ModifierAccesUtilisateursComponent } from './modifier-acces-utilisateurs.component';

@Component({
    selector: 'modifier-erabliere',
    templateUrl: './modifier-erabliere.component.html',
})
export class ModifierErabliereComponent implements OnInit {
    @ViewChild(ErabliereFormComponent) erabliereForm?: ErabliereFormComponent;
    @ViewChild(ModifierAccesUtilisateursComponent) modifierAccesUtilisateurs?: ModifierAccesUtilisateursComponent;
    modalTitle: string = "Modifier une érablière";
    @Output() shouldReloadErablieres = new EventEmitter();
    @Input() authEnabled: Boolean = false;
    @Input() idErabliere?: any;

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