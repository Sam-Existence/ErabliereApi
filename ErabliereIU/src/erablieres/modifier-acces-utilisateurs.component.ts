import { Component, Input, OnInit } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';
import { PutCustomerAccess } from 'src/model/putCustomerAccess';

@Component({
    selector: 'modifier-acces-utilisateurs',
    templateUrl: 'modifier-acces-utilisateurs.component.html',
})

export class ModifierAccesUtilisateursComponent implements OnInit {
    customersAccess: CustomerAccess[] = [];
    erreurChargementDroits: Boolean = false;
    erreurAjoutAcces: Boolean = false;
    displayNewLine: Boolean = false;
    displayEditAccess: Boolean = false;
    newCustomerAccess: PutCustomerAccess = new PutCustomerAccess()
    @Input() idErabliere: any;
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

    addUserAccess() {
        this.displayNewLine = true;
        this.newCustomerAccess = new PutCustomerAccess();
    }

    hideAddUserAccess() {
        this.displayNewLine = false;
    }

    creerAcces() {
        this.erreurAjoutAcces = false;
        this.newCustomerAccess.idErabliere = this.idErabliere;
        this.newCustomerAccess.customerErablieres[0].action = 0;
        this._api.putCustomerAccess(this.idErabliere, this.newCustomerAccess).then(() => {
            this.refreashAccess(this.idErabliere);
            this.displayNewLine = false;
            this.newCustomerAccess = new PutCustomerAccess();
        }).catch(error => {
            this.erreurAjoutAcces = true;
            throw error;
        });
    }

    customerSelected(customer: Customer) {
        console.log("CustomerSelected " + customer.id);
        this.newCustomerAccess.customerErablieres[0].idCustomer = customer.id;
    }

    modifierAcces(idAccess: any) {
        this.displayEditAccess = true;
    }

    supprimer(customerAccess?: CustomerAccess) {
        if (customerAccess != undefined) {
            this._api.deleteCustomerAccess(customerAccess.idErabliere, customerAccess.id).then(() => {
                this.refreashAccess(customerAccess.idErabliere);
            });
        }
    }
}