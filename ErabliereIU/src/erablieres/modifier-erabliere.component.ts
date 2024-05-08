// This a component that allows to add a new erabliere
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component'
import { ModifierAccesUtilisateursComponent } from './modifier-acces-utilisateurs.component';
import { NgIf } from '@angular/common';
import { ErabliereFormComponent as ErabliereFormComponent_1 } from './erabliere-form.component';

@Component({
    selector: 'modifier-erabliere',
    templateUrl: './modifier-erabliere.component.html',
    standalone: true,
    imports: [
        ErabliereFormComponent_1,
        NgIf,
        ModifierAccesUtilisateursComponent,
    ],
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
                    if (this.erabliereForm != undefined) {
                        this.erabliereForm.generalError = undefined;
                        this.erabliereForm.errorObj = undefined;
                    }
                    this.shouldReloadErablieres.emit();
                }).catch(error => {
                    if (this.erabliereForm != undefined) {
                        if (error.status == 400) {
                            this.erabliereForm.errorObj = error;
                            this.erabliereForm.generalError = undefined;
                        } 
                        else {
                            this.erabliereForm.generalError = "Une erreur est survenue lors de la modification de l'érablière. Veuillez réessayer plus tard."
                        }
                    }
                })
            }
        }
    }

    showDeleteErabliere() {
        this.afficherSectionDeleteErabliere = true;
    }

    deleteErabliere() {
        const e = `Voulez-vous vraiment supprimer l'érablière avec l'identifiant ${this.idErabliere} ? Cette action est irréversible.`;

        if (confirm(e)) {
            var erabliere = this.erabliereForm?.erabliere;
            if (erabliere != undefined) {
                this._api.deleteErabliere(this.idErabliere, erabliere).then(() => {
                    this.afficherSectionDeleteErabliere = true;
                    this.shouldReloadErablieres.emit({event: 'delete'});

                    // Hide the modal with the ID modifierErabliereFormModal
                    var modal = document.getElementById("modifierErabliereFormModal");
                    if (modal != null) {
                        modal.style.display = "none";

                        // remove the div with class modal-backdrop
                        var modalBackdrop = document.getElementsByClassName("modal-backdrop");
                        if (modalBackdrop.length > 0) {
                            modalBackdrop[0].remove();
                        }
                    }

                }).catch(error => {
                    console.error(error);
                    alert("Une erreur est survenue lors de la suppression de l'érablière. Veuillez réessayer plus tard.");
                });
            }
        }
    }

    onHideModal() {
        this.afficherSectionDeleteErabliere = false;
    }
}