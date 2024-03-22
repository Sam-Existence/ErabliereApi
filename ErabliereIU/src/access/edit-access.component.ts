import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { CustomerAccess } from 'src/model/customerAccess';
import { PutCustomerAccess } from 'src/model/putCustomerAccess';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';

@Component({
    selector: 'edit-access',
    templateUrl: 'edit-access.component.html',
    standalone: true,
    imports: [NgIf, ReactiveFormsModule, FormsModule]
})
export class EditAccessComponent implements OnInit {
    @Input() displayEditAccess: Boolean = false;
    @Input() acces: CustomerAccess = new CustomerAccess();
    accessInternal: CustomerAccess = new CustomerAccess();
    @Output() shouldUpdateAccess = new EventEmitter<Boolean>();

    constructor(private _api: ErabliereApi) { }

    ngOnInit() { 
        this.accessInternal = { ...this.acces };
    }

    getAccessText(access?: number): string {
        switch (access) {
            case 0:
                return $localize `:aucunAcces:Aucun`;
            case 1:
                return $localize `:lectureAcces:Lecture`;
            case 2:
                return $localize `:creationAcces:Création`;
            case 3:
                return $localize `:lectureCreationAcces:Lecture et création`;
            case 4:
                return $localize `:modificaitonAcces:Modification`;
            case 5:
                return $localize `:lectureModificationAcces:Lecture et modification`;
            case 6:
                return $localize `:creationModificationAcces:Création et modification`;
            case 7:
                return $localize `:lectureCreationModificationAcces:Lecture, création et modification`;
            case 8:
                return $localize `:suppressionAcces:Suppression`;
            case 9:
                return $localize `:lectureSuppressionAcces:Lecture et suppression`;
            case 10:
                return $localize `:creationSuppressionAcces:Création et suppression`;
            case 11:
                return $localize `:lectureCreationSuppressionAcces:Lecture, création et suppression`;
            case 12:
                return $localize `:modificationSuppressionAcces:Modification et suppression`;
            case 13:
                return $localize `:lectureModificationSuppressionAcces:Lecture, modification et suppression`;
            case 14:
                return $localize `:creationModificationSuppressionAcces:Création, modification et suppression`;
            case 15:
                return $localize `:lectureCreationModificationSuppressionAcces:Lecture, création, modification et suppression`;
            default:
                return $localize `:aucunAcces:Aucun`;
        }
    }

    updateAccess() {
        if (this.acces != undefined) {

            var putModel = new PutCustomerAccess();
            putModel.idErabliere = this.acces.idErabliere;
            putModel.customerErablieres = [];
            putModel.customerErablieres.push({
                idCustomer: this.acces.idCustomer,
                action: 1,
                access: this.accessInternal.access
            });

            this._api.putCustomerAccess(this.acces.idErabliere, putModel).then(() => {
                this.displayEditAccess = false;
                this.shouldUpdateAccess.emit(true);
            }).catch(error => {
                throw error;
            });
        }
    }
}