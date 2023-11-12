import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Customer } from 'src/model/customer';
import { NgFor } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
    selector: 'select-customer',
    templateUrl: 'select-customer.component.html',
    standalone: true,
    imports: [ReactiveFormsModule, NgFor]
})

export class SelectCustomerComponent implements OnInit {
    @ViewChild('select') selectRef?: ElementRef;
    customers: Customer[] = [];
    @Output() customerSelected = new EventEmitter<Customer>();

    constructor(private _api: ErabliereApi) { }

    ngOnInit() { 
        this._api.getCustomers().then(customers => {
            this.customers = customers;
        })
        .catch(error => {
            this.customers = [];
            throw error;
        });
    }

    onSelectChange(event: any) {
        this.customerSelected.emit(this.customers.find(c => c.id == event.target.value));
    }
}