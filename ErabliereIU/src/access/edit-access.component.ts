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