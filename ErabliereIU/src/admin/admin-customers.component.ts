import {Component, OnInit} from '@angular/core';
import {CustomerListComponent} from "./customer/customer-list/customer-list.component";
import {ErabliereApi} from "../core/erabliereapi.service";
import {Customer} from "../model/customer";

@Component({
  selector: 'admin-customers',
  standalone: true,
    imports: [
        CustomerListComponent
    ],
  templateUrl: './admin-customers.component.html',
})
export class AdminCustomersComponent implements OnInit {
  customers: Customer[] = [];

  constructor(private _api: ErabliereApi) { }

  ngOnInit() {
    this.chargerCustomers();
  }

  chargerCustomers() {
    this._api.getCustomersAdmin().then(customers => {
      this.customers = customers;
    }).catch(error => {
      this.customers = [];
      throw error;
    });
  }

  supprimerCustomer(customer: Customer) {
    if (confirm("Voulez-vous vraiment supprimer le compte de " + customer.name + " ? ")) {
      this._api.deleteCustomer(customer.id)
        .then(a => {
          this.chargerCustomers();
        });
    }
  }
}
