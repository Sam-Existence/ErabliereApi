import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Customer} from "../../../model/customer";

@Component({
  selector: 'customer-list',
  standalone: true,
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.css'
})
export class CustomerListComponent {
    @Input() customers: Customer[] = [];
    @Output() customerASupprimer: EventEmitter<Customer> = new EventEmitter();
    @Output() customerAModifier: EventEmitter<Customer> = new EventEmitter();

    signalerSuppression(customer: Customer) {
      this.customerASupprimer.emit(customer);
  }

    signalerModification(customer: Customer) {
      this.customerAModifier.emit(customer);
    }
}
