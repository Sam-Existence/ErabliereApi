import { Component } from '@angular/core';
import {CustomerListComponent} from "./customer/customer-list/customer-list.component";

@Component({
  selector: 'admin-customers',
  standalone: true,
    imports: [
        CustomerListComponent
    ],
  templateUrl: './admin-customers.component.html',
})
export class AdminCustomersComponent {

}
