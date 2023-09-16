import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { CustomerAccess } from 'src/model/customerAccess';
import { PutCustomerAccess } from 'src/model/putCustomerAccess';

@Component({
    selector: 'edit-access',
    templateUrl: 'edit-access.component.html'
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
                return "Aucun";
            case 1:
                return "Lecture";
            case 2:
                return "Création";
            case 3:
                return "Lecture et création"
            case 4:
                return "Modification";
            case 5:
                return "Lecture et modification";
            case 6:
                return "Création et modification";
            case 7:
                return "Lecture, création et modification";
            case 8:
                return "Suppression";
            case 9:
                return "Lecture et suppression";
            case 10:
                return "Création et suppression";
            case 11:
                return "Lecture, création et suppression";
            case 12:
                return "Modification et suppression";
            case 13:
                return "Lecture, modification et suppression";
            case 14:
                return "Création, modification et suppression";
            case 15:
                return "Lecture, création, modification et suppression";
            default:
                return "Aucun";
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