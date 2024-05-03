import {Component, EventEmitter, Output} from '@angular/core';
import {Customer} from "../../../model/customer";
import {ErabliereApi} from "../../../core/erabliereapi.service";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'customer-list',
  standalone: true,
    imports: [
        NgForOf
    ],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.css'
})
export class CustomerListComponent {
    @Output() customerSelected = new EventEmitter<Customer>();
    customers: Customer[] = [];
    constructor(private _api: ErabliereApi) { }

    ngOnInit() {
        this._api.getCustomersAdmin().then(customers => {
            this.customers = customers;
        })
            .catch(error => {
                this.customers = [];
                throw error;
            });
    }
}
