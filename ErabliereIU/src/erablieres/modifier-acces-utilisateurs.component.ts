import { Component, Input, OnInit } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { CustomerAccess } from 'src/model/customerAccess';

@Component({
    selector: 'modifier-acces-utilisateurs',
    templateUrl: 'modifier-acces-utilisateurs.component.html',
})

export class ModifierAccesUtilisateursComponent implements OnInit {
    customersAccess: CustomerAccess[] = [];
    erreurChargementDroits: Boolean = false;
    constructor(private _api: ErabliereApi) { }

    ngOnInit() { 
        
    }

    refreashAccess(idErabliere: any) {
        this.erreurChargementDroits = false;
        this._api.getCustomersAccess(idErabliere).then(customersAccess => {
            this.customersAccess = customersAccess;
        })
        .catch(error => {
            this.customersAccess = [];
            this.erreurChargementDroits = true;
            throw error;
        });
    }

    supprimer(customerAccess: CustomerAccess) {
        this._api.deleteCustomerAccess(customerAccess.idErabliere, customerAccess.id).then(() => {
            this.refreashAccess(customerAccess.idErabliere);
        });
    }
}