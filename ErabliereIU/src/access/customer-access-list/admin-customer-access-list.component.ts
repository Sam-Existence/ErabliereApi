import {Component, EventEmitter, Input, Output} from '@angular/core';
import {CustomerAccess} from "../../model/customerAccess";
import {ErabliereApi} from "../../core/erabliereapi.service";
import {ViewCustomerAccessRowComponent} from "./view-customer-access-row/view-customer-access-row.component";
import {EditCustomerAccessRowComponent} from "./edit-customer-access-row/edit-customer-access-row.component";
import {AddCustomerAccessRowComponent} from "./add-customer-access-row/add-customer-access-row.component";

@Component({
  selector: 'admin-customer-access-list',
  standalone: true,
    imports: [
        ViewCustomerAccessRowComponent,
        EditCustomerAccessRowComponent,
        AddCustomerAccessRowComponent
    ],
  templateUrl: './customer-access-list.component.html',
  styleUrl: './customer-access-list.component.css',
})
export class AdminCustomerAccessListComponent {
    @Input() idCustomer?: string;

    @Output() changementAcces: EventEmitter<CustomerAccess[]> = new EventEmitter();

    customersAccess: CustomerAccess[] = [];

    displayEdits: { [id: string]: boolean } = {};

    erreurChargementDroits: Boolean = false;
    erreur: string = '';

    displayNewLine: Boolean = false;

    constructor(private _api: ErabliereApi) {
    }

    ngOnInit() {
        this.refreshAccess(this.idCustomer);
    }

    updateDisplayEdits() {
        this.customersAccess.forEach((customer) => {
            if (!this.displayEdits[customer.idErabliere]) {
                this.displayEdits[customer.idErabliere] = false;
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
            this.displayEdits[acces.idErabliere] = true;
        }
    }

    annulerModifierAcces(acces: CustomerAccess) {
        if (acces.idCustomer) {
            this.displayEdits[acces.idErabliere] = false;
        }
    }

    terminerModifierAcces(acces: CustomerAccess) {
        if (acces.idErabliere) {
            this._api.putAdminCustomerAccess(acces).then(() => {
                this.erreur = "";
                this.displayEdits[acces.idErabliere] = false;
            }).catch((error) => {
                if (error.status === 403) {
                    this.erreur = "Vous n'avez pas le droit de modification.";
                } else {
                    this.erreur = "Une erreur est survenue lors de la modification d'un accès.";
                }
            });
        }
    }

    refreshAccess(idCustomer?: string) {
        if (!idCustomer) {
            return;
        }

        this.erreurChargementDroits = false;
        return this._api.getAdminCustomerAccess(idCustomer).then(customersAccess => {
            this.customersAccess = customersAccess;
            this.changementAcces.emit(this.customersAccess);
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
        if (confirm("Voulez-vous vraiment supprimer l'accès de " + access.erabliere?.nom + " ? ")) {
            this._api.deleteAdminCustomerAccess(access.idErabliere, access.idCustomer).then(() => {
                this.erreur = "";
                this.refreshAccess(this.idCustomer);
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
        this._api.postAdminCustomerAccess(access).then(() => {
            this.erreur = "";
            this.refreshAccess(this.idCustomer);
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
