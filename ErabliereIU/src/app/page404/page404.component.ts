import { Component } from '@angular/core';
import {CustomerListComponent} from "../../admin/customer/customer-list/customer-list.component";
import {ModifierCustomerComponent} from "../../admin/customer/modifier-customer/modifier-customer.component";

@Component({
  selector: 'page404',
  standalone: true,
    imports: [
        CustomerListComponent,
        ModifierCustomerComponent
    ],
  templateUrl: './page404.component.html'
})
export class Page404Component {

}
