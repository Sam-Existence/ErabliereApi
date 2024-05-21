import { Component } from '@angular/core';
import {RouterLink, RouterLinkActive} from "@angular/router";
import {AgoraCallServiceComponent} from "../../client-view/agora-call-service/agora-call-service.component";
import {ConnectionButtonComponent} from "../../../authorisation/connection-button/connection-button.component";
import {EnvironmentService} from "../../../environments/environment.service";

@Component({
  selector: 'admin-nav-bar',
  standalone: true,
    imports: [
        RouterLink,
        RouterLinkActive,
        AgoraCallServiceComponent,
        ConnectionButtonComponent
    ],
  templateUrl: './admin-nav-bar.component.html'
})
export class AdminNavBarComponent {
    useAuthentication: boolean = false;

    constructor(private environmentService: EnvironmentService) {
        this.useAuthentication = this.environmentService.authEnable ?? false;
    }
}
