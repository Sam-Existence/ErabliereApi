import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { CustomerAccess } from 'src/model/customerAccess';
import { ViewAccessRowComponent } from './view-access-row/view-access-row.component';
import { EditAccessRowComponent } from "./edit-access-row/edit-access-row.component";
import { AddAccessRowComponent } from "./add-access-row/add-access-row.component";
import {Customer} from "../model/customer";

@Component({
    selector: 'access-list',
    templateUrl: './access-list.component.html',
    standalone: true,
    imports: [
        ViewAccessRowComponent,
        EditAccessRowComponent,
        AddAccessRowComponent,
    ],
})

export class AccessListComponent implements OnChanges {
    @Input() idErabliere?: string;
    @Input() customersAccess: CustomerAccess[] = [];

    displayEdits: { [id: string]: boolean } = {};

    erreurChargementDroits: Boolean = false;
    erreurAjoutAcces: Boolean = false;

    displayNewLine: Boolean = false;

    constructor(private _api: ErabliereApi) {
    }

    ngOnChanges(changes:SimpleChanges) {
        if(changes['idErabliere']) {
            this.refreshAccess(this.idErabliere);
        }
    }

    updateDisplayEdits() {
        this.customersAccess.forEach((customer) => {
           if (!this.displayEdits[customer.idCustomer]) {
               this.displayEdits[customer.idCustomer] = false;
           }
        });
    }

    isDisplayEditForm(customerId?: string): boolean {
        if (!customerId) {
            return false;
        }

        return this.displayEdits[customerId];
    }

    showModifierAcces(acces: CustomerAccess) {
        if (acces.idCustomer) {
            this.displayEdits[acces.idCustomer] = true;
        }
    }

    annulerModifierAcces(acces: CustomerAccess) {
        if (acces.idCustomer) {
            this.displayEdits[acces.idCustomer] = false;
        }
    }

    async terminerModifierAcces(acces: CustomerAccess) {
        if (acces.idCustomer) {
            await this._api.putCustomerAccess(acces);
            this.displayEdits[acces.idCustomer] = false;
        }
    }

    refreshAccess(idErabliere?: string) {
        if (!idErabliere) {
            return;
        }

        this.erreurChargementDroits = false;
        return this._api.getCustomersAccess(idErabliere).then(customersAccess => {
            this.customersAccess = customersAccess;
            this.updateDisplayEdits();
        })
        .catch(error => {
            this.customersAccess = [];
            this.displayEdits = {};
            this.erreurChargementDroits = true;
            throw error;
        });
    }

    addUserAccess() {
        this.displayNewLine = true;
    }

    hideAddUserAccess() {
        this.displayNewLine = false;
    }

    supprimerAcces(access: CustomerAccess) {
        if (confirm("Voulez-vous vraiment supprimer l'accès de " + access.customer?.name + " ? ")) {
            this._api.deleteCustomerAccess(access.idErabliere, access.idCustomer).then(() => {
                this.refreshAccess(this.idErabliere);
            });
        }
    }

    creerAcces(access: CustomerAccess) {
        this.erreurAjoutAcces = false;

        this._api.postCustomerAccess(access).then(() => {
            this.refreshAccess(this.idErabliere);
            this.displayNewLine = false;
        }).catch(error => {
            this.erreurAjoutAcces = true;
            throw error;
        });
    }
}
