import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Customer } from 'src/model/customer';
import {FormBuilder, FormControl, ReactiveFormsModule, UntypedFormGroup, Validators} from '@angular/forms';

@Component({
    selector: 'select-customer',
    templateUrl: 'select-customer.component.html',
    standalone: true,
    imports: [ReactiveFormsModule]
})

export class SelectCustomerComponent implements OnInit {
    @Input() form: UntypedFormGroup;
    @Output() customerSelected = new EventEmitter<Customer>();

    customers: Customer[] = [];

    constructor(private _api: ErabliereApi, private formBuilder: FormBuilder) {
        this.form = this.formBuilder.group({
            customer: new FormControl(null,
                {
                    validators: [Validators.required],
                    updateOn: 'blur'
                })
        });
    }

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
        let customer;
        if (event.target.value) {
            customer = this.customers.find(c => c.id === event.target.value);
        }
        this.customerSelected.emit(customer);
    }
}
