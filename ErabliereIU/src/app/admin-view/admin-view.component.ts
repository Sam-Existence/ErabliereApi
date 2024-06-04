import { Component } from '@angular/core';
import {RouterOutlet} from "@angular/router";
import {AdminNavBarComponent} from "./admin-nav-bar/admin-nav-bar.component";

@Component({
  selector: 'admin-view',
  standalone: true,
    imports: [
        RouterOutlet,
        AdminNavBarComponent
    ],
  templateUrl: './admin-view.component.html'
})
export class AdminViewComponent {

}
