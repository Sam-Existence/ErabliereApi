import {Component, EventEmitter, Input, Output, OnInit} from '@angular/core';
import {Customer} from "../../../model/customer";
import {ErabliereApi} from "../../../core/erabliereapi.service";
import {ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {InputErrorComponent} from "../../../formsComponents/input-error.component";
import {NgIf} from "@angular/common";

@Component({
  selector: 'modifier-customer-modal',
  standalone: true,
  imports: [
    InputErrorComponent,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './modifier-customer.component.html',
})
export class ModifierCustomerComponent implements OnInit {
  @Input() customer: Customer | null = null;
  @Output() needToUpdate : EventEmitter<boolean> = new EventEmitter();

  customerForm: UntypedFormGroup;
  errorObj?: any;
  generalError?: string | null;

  constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
    this.customerForm = this.fb.group({});
  }

  ngOnInit() : void {
    this.initializeForm();
  }

  initializeForm() {
    this.customerForm = this.fb.group({
      nom: [this.customer?.name, Validators.required],
      email : { value: this.customer?.email, disabled: true }
    });
  }

  onModifier() {
    if (this.customer && this.customerForm.valid) {

      this.customer.name = this.customerForm.controls['nom'].value;

      this._api.putCustomer(this.customer.id, this.customer)
        .then(r => {
          this.errorObj = null;
          this.generalError = null;
          this.customerForm.reset();
          this.needToUpdate.emit(true);
        })
        .catch(e => {
          if (e.status == 400) {
            this.errorObj = e;
            this.generalError = "Le nom ne doit pas être vide";
          }
          else if (e.status == 404) {
            this.errorObj = null;
            this.generalError = "L'utilisateur n'existe pas."
          }
          else {
            this.errorObj = null;
            this.generalError = "Une erreur est survenue."
          }
        });
    }
  }

  onAnnuler() {
    this.needToUpdate.emit(false);
  }
}
