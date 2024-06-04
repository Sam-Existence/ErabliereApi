import { Component, Output, EventEmitter, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component'
import { ErabliereFormComponent as ErabliereFormComponent_1 } from './erabliere-form.component';

@Component({
    selector: 'ajouter-erabliere',
    templateUrl: './ajouter-erabliere.component.html',
    standalone: true,
    imports: [ErabliereFormComponent_1],
})
export class AjouterErabliereComponent {
    @ViewChild(ErabliereFormComponent) erabliereForm?: ErabliereFormComponent
    modalTitle: string = "Ajouter une érablière"
    @Output() shouldReloadErablieres = new EventEmitter()

    constructor(private _api: ErabliereApi) { }

    ajouterErabliere() {
        if (this.erabliereForm != undefined && this.erabliereForm.erabliere != undefined) {
            let erabliere = this.erabliereForm.erabliere

            this._api.postErabliere(erabliere).then(() => {
                if (this.erabliereForm != undefined) {
                    this.erabliereForm.generalError = undefined
                    this.erabliereForm.errorObj = undefined
                    this.erabliereForm.erabliere = this.erabliereForm.getDefaultErabliere()
                }
                this.shouldReloadErablieres.emit();
            }).catch(error => {
                if (this.erabliereForm != undefined) {
                    if (error.status == 400) {
                        this.erabliereForm.errorObj = error
                        this.erabliereForm.generalError = undefined
                    }
                    else {
                        this.erabliereForm.generalError = "Une erreur est survenue lors de l'ajout de l'érablière. Veuillez réessayer plus tard."
                    }
                }
            });
        }
    }
}
