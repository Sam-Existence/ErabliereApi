import {Component, OnInit} from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { RouterOutlet } from '@angular/router';
import { ErabliereAIComponent } from 'src/erabliereai/erabliereai-chat.component';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { MsalService } from '@azure/msal-angular';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  standalone: true,
  imports: [
    RouterOutlet,
    EntraRedirectComponent,
    ErabliereAIComponent
  ]
})
export class AppComponent implements OnInit {
  private _pagesSansMenu = ["apropos"];
  erabliereAIEnable: boolean = false;
  erabliereAIUserRole: boolean = false;
  authService: IAuthorisationSerivce;

  constructor(private api: ErabliereApi, authServiceFactory: AuthorisationFactoryService, private msalService: MsalService) {
    this.authService = authServiceFactory.getAuthorisationService();
  }

  ngOnInit(): void {
    this.api.getOpenApiSpec().then(spec => {
        this.erabliereAIEnable = spec.paths['/ErabliereAI/Conversations'] !== undefined;
        console.log("ErabliereAIEnable: " + this.erabliereAIEnable);
    })
    .catch(err => {
        console.error(err);
    });

    // get the user role to see if it got the ErabliereAIUser role
    // if so, enable the chat widget
    if (this.authService.type == "AzureAD") {
      this.checkRoleErabliereAI();
      this.authService.loginChanged.subscribe((val) => {
        this.checkRoleErabliereAI();
      });
    }
  }

  private checkRoleErabliereAI() {
    console.log("CheckRoleErabliereAI");
    const account = this.msalService.instance.getActiveAccount();
    if (account?.idTokenClaims) {
      const roles = account?.idTokenClaims['roles'];
      if (roles != null) {
        this.erabliereAIUserRole = roles.includes("ErabliereAIUser");
      }
      else {
        this.erabliereAIUserRole = false;
      }
    }
    else {
      this.erabliereAIUserRole = false;
    }
    console.log("ErabliereAIUserRole: " + this.erabliereAIUserRole);
  }
}
