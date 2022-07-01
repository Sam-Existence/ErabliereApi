import { Component, OnInit } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { CustomerAccess } from 'src/model/customerAccess';

@Component({
    selector: 'modifier-acces-utilisateurs',
    templateUrl: 'modifier-acces-utilisateurs.component.html',
})

export class ModifierAccesUtilisateursComponent implements OnInit {
    customersAccess: CustomerAccess[] = [];
    constructor(private _api: ErabliereApi) { }

    ngOnInit() { 
        
    }

    refreashAccess(idErabliere: any) {
        this._api.getCustomersAccess(idErabliere).then(customersAccess => {
            this.customersAccess = customersAccess;
        });
    }

    supprimer(CustomerAccess: CustomerAccess) {
        // this._api.deleteCustomerAccess(CustomerAccess.id).then(() => {
        //     this.refreashAccess(CustomerAccess.idErabliere);
        // });
    }
}