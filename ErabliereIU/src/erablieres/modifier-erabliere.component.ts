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
    afficherSectionDeleteErabliere: boolean = false;

    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
        this.afficherSectionDeleteErabliere = false;
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

    showDeleteErabliere() {
        this.afficherSectionDeleteErabliere = true;
    }

    deleteErabliere() {
        if (confirm("Voulez-vous vraiment supprimer cette érablière ? Cette action est irréversible.")) {
            var erabliere = this.erabliereForm?.erabliere;
            if (erabliere != undefined) {
                this._api.deleteErabliere(this.idErabliere, erabliere).then(() => {
                    this.afficherSectionDeleteErabliere = true;
                    this.shouldReloadErablieres.emit();
                });
            }
        }	
    }
}