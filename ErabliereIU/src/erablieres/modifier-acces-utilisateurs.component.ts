import { Component, Input } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';
import { PutCustomerAccess } from 'src/model/putCustomerAccess';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SelectCustomerComponent } from '../customer/select-customer.component';
import { EditAccessComponent } from '../access/edit-access.component';
import { NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'modifier-acces-utilisateurs',
    templateUrl: 'modifier-acces-utilisateurs.component.html',
    standalone: true,
    imports: [
        NgIf,
        NgFor,
        EditAccessComponent,
        SelectCustomerComponent,
        ReactiveFormsModule,
        FormsModule,
    ],
})

export class ModifierAccesUtilisateursComponent {
    customersAccess: CustomerAccess[] = [];
    erreurChargementDroits: Boolean = false;
    erreurAjoutAcces: Boolean = false;
    displayNewLine: Boolean = false;
    displayEditAccess: Boolean = false;
    displaySection: Boolean = false;
    newCustomerAccess: PutCustomerAccess = new PutCustomerAccess()
    @Input() idErabliere: any;
    constructor(private _api: ErabliereApi) { }

    changeDisplaySection() {
        this.displaySection = !this.displaySection;
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

    showEditForm() {
        this.displayEditAccess = true;
    }

    hideEditForm() {
        this.displayEditAccess = false;
    }

    supprimer(customerAccess?: CustomerAccess) {
        if (customerAccess != undefined) {
            this._api.deleteCustomerAccess(customerAccess.idErabliere, customerAccess.idCustomer).then(() => {
                this.refreashAccess(customerAccess.idErabliere);
            });
        }
    }

    shouldRefreshAccessAfterUpdate(shouldRefreshAccess: Boolean) {
        this.displayEditAccess = false;
        if (shouldRefreshAccess) {
            this.refreashAccess(this.idErabliere);
        }
    }
}
