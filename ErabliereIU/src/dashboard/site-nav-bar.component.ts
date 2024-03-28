import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from '../environments/environment.service';
import { UrlModel } from '../model/urlModel';
import { NgFor, NgIf } from '@angular/common';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { AgoraCallServiceComponent } from './agora-call-service.component';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { MsalService } from '@azure/msal-angular';

@Component({
    selector: 'site-nav-bar',
    template: 'site-nav-bar.component.html',
    standalone: true,
    imports: [NgFor, NgIf, RouterOutlet, RouterLink, RouterLinkActive, AgoraCallServiceComponent]
})
export class SiteNavBarComponent implements OnInit {
  useAuthentication: boolean = false;
  isLoggedIn: boolean;
  urls: UrlModel[]
  tenantId?: string;
  idErabliereSelectionnee?: any;
  private _authService: IAuthorisationSerivce
  @Input() thereIsAtLeastOneErabliereSubject: Subject<boolean> = new Subject<boolean>();
  thereIsAtLeastOneErabliere: boolean = false;
  callFeatureEnableForUser: boolean = false;
  callFeatureEnable: boolean = false;

  constructor(
      authFactoryService: AuthorisationFactoryService, 
      private environmentService: EnvironmentService, 
      private cdr: ChangeDetectorRef,
      private router: Router,
      private api: ErabliereApi,
      private msalService: MsalService) {
    this._authService = authFactoryService.getAuthorisationService()
    this.useAuthentication = environmentService.authEnable ?? false;
    this.thereIsAtLeastOneErabliere = false
    this.isLoggedIn = false
    this.urls = []
  }
  
  ngOnInit(): void {
    this.thereIsAtLeastOneErabliereSubject.subscribe((val) => {
      this.thereIsAtLeastOneErabliere = val;
    });
    this._authService.isLoggedIn().then(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
    this._authService.loginChanged.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      this.cdr.detectChanges();
    });
    this.urls = this.environmentService.additionnalUrls ?? [];
    this.tenantId = this.environmentService.tenantId;

    // update the idErabliereSelectionnee when the route changes
    this.router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        const url = val.url;
        const urlParts = url.split('/');
        if (urlParts.length >= 3) {
          const idErabliereSelectionnee = urlParts[2];
          this.idErabliereSelectionnee = idErabliereSelectionnee;
        }
        else {

        }
      }
    });

    this.checkApiCallFeatureEnable();
  }

  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }

  checkApiCallFeatureEnable() {
    // look at the openapi spec to see if the call endpoint is enable
    this.api.getOpenApiSpec().then(spec => {
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
    const account = this.msalService.instance.getActiveAccount();
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
}
