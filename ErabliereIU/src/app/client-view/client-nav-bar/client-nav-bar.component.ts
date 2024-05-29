import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from '../../../environments/environment.service';
import { UrlModel } from '../../../model/urlModel';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AgoraCallServiceComponent } from '../agora-call-service/agora-call-service.component';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { MsalService } from '@azure/msal-angular';
import {ConnectionButtonComponent} from "../../../authorisation/connection-button/connection-button.component";

@Component({
    selector: 'client-nav-bar',
    templateUrl: 'client-nav-bar.component.html',
    standalone: true,
    imports: [RouterOutlet, RouterLink, RouterLinkActive, AgoraCallServiceComponent, ConnectionButtonComponent]
})
export class ClientNavBarComponent implements OnInit {
  private _authService: IAuthorisationSerivce

  @Input() idErabliereSelectionee?: string;
  @Input() thereIsAtLeastOneErabliere: boolean;

  useAuthentication: boolean = false;
  isLoggedIn: boolean;
  urls?: UrlModel[]
  callFeatureEnableForUser: boolean = false;
  callFeatureEnable: boolean = false;
  isAdminUser: boolean = false;

  constructor(
      authFactoryService: AuthorisationFactoryService,
      private _environmentService: EnvironmentService,
      private _api: ErabliereApi,
      private _msalService: MsalService) {
    this._authService = authFactoryService.getAuthorisationService()
    this.useAuthentication = this._environmentService.authEnable ?? false;
    this.thereIsAtLeastOneErabliere = false
    this.isLoggedIn = !this.useAuthentication
    this.urls = this._environmentService.additionnalUrls;
  }

  ngOnInit(): void {
    this.checkApiCallFeatureEnable();
    this.checkRoleAdmin();
  }

  checkApiCallFeatureEnable() {
    // look at the openapi spec to see if the call endpoint is enable
    this._api.getOpenApiSpec().then(spec => {
      this.callFeatureEnable = spec.paths['/Calls/GetAppId'] !== undefined;
      console.log("CallFeatureEnable: " + this.callFeatureEnable);
    })
    .catch(err => {
        console.error(err);
    });

    this.checkUserRollForFeatureCall();
  }

  checkUserRollForFeatureCall() {
    // look at the user roll to see if the call feature is enable
    if (this._authService.type == "AzureAD") {
      this.checkRoleErabliereCalls();
      this._authService.loginChanged.subscribe((val) => {
        this.checkRoleErabliereCalls();
      });
    }
  }

  private checkRoleErabliereCalls() {
    console.log("checkRoleErabliereCalls");
    const account = this._msalService.instance.getActiveAccount();
    if (account?.idTokenClaims) {
      const roles = account?.idTokenClaims['roles'];
      if (roles != null) {
        this.callFeatureEnableForUser = roles.includes("ErabliereCalls");
      }
      else {
        this.callFeatureEnableForUser = false;
      }
    }
    else {
      this.callFeatureEnableForUser = false;
    }
    console.log("callFeatureEnableForUser: " + this.callFeatureEnableForUser);
  }

  private checkRoleAdmin() {
      const account = this._msalService.instance.getActiveAccount();
      this.isAdminUser = false;
      if (account?.idTokenClaims) {
          const roles = account?.idTokenClaims['roles'];
          if (roles != null) {
              this.isAdminUser = roles.includes("administrateur");
          }
      }
  }

  onLoginChange(loginState: boolean) {
      this.isLoggedIn = loginState;
      this.checkRoleAdmin();
  }
}
