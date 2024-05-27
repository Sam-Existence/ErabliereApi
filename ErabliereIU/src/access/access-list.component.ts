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
    erreur: string = '';

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

    terminerModifierAcces(acces: CustomerAccess) {
        if (acces.idCustomer) {
            this._api.putCustomerAccess(acces).then(() => {
                this.erreur = "";
                this.displayEdits[acces.idCustomer] = false;
            }).catch((error) => {
                if (error.status === 403) {
                    this.erreur = "Vous n'avez pas le droit de modification.";
                } else {
                    this.erreur = "Une erreur est survenue lors de la modification d'un accès.";
                }
            });
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
                this.erreur = "";
                this.refreshAccess(this.idErabliere);
            }).catch(error => {
                if (error.status === 403) {
                    this.erreur = "Vous n'avez pas le droit de suppression.";
                } else {
                    this.erreur = "Une erreur est survenue lors de la suppression d'un accès.";
                }
                throw error;
            });
        }
    }

    creerAcces(access: CustomerAccess) {
        this._api.postCustomerAccess(access).then(() => {
            this.erreur = "";
            this.refreshAccess(this.idErabliere);
            this.displayNewLine = false;
        }).catch(error => {
            if (error.status === 403) {
                this.erreur = "Vous n'avez pas le droit de création.";
            } else {
                this.erreur = "Une erreur est survenue lors de l'ajout d'un accès.";
            }
            throw error;
        });
    }
}