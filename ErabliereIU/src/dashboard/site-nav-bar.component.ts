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
    template: `
        <nav class="navbar navbar-expand-lg navbar-light bd-navbar">
            <div class="container-fluid">
            <h2 class="ms-4 me-5">Érablière IU</h2>
            <button class="navbar-toggler" 
                    type="button" 
                    data-bs-toggle="collapse" 
                    data-bs-target=".navbar-collapse" 
                    aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-lg-inline-flex flex-lg-row">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0 me-auto">
                    <li class="nav-item">
                        <a class="nav-link" routerLink="/e/{{idErabliereSelectionnee}}/graphiques" routerLinkActive="active" ariaCurrentWhenActive="page">Graphique</a>
                    </li>
                    <li *ngFor="let url of urls" class="nav-item">
                        <a class="nav-link" href="{{ url.href }}" role="button" target="_blank" rel="noopener noreferrer">{{ url.text }}</a>
                    </li>
                    <li *ngIf="isLoggedIn && thereIsAtLeastOneErabliere" class="nav-item">
                        <a id="nav-menu-alerte-button" class="nav-link" routerLink="e/{{idErabliereSelectionnee}}/alertes" routerLinkActive="active" ariaCurrentWhenActive="page">Alerte</a>
                    </li>
                    <li *ngIf="isLoggedIn && thereIsAtLeastOneErabliere" class="nav-item">
                        <a id="nav-menu-notes-button" class="nav-link" routerLink="e/{{idErabliereSelectionnee}}/notes" routerLinkActive="active" ariaCurrentWhenActive="page">Notes</a>
                    </li>
                    <li *ngIf="isLoggedIn && thereIsAtLeastOneErabliere" class="nav-item">
                        <a class="nav-link" routerLink="e/{{idErabliereSelectionnee}}/documentations" routerLinkActive="active" ariaCurrentWhenActive="page">Documentation</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" routerLink="/apropos" routerLinkActive="active" ariaCurrentWhenActive="page">À Propos</a>
                    </li>
                </ul>
                <span [hidden]="!useAuthentication">
                    <agora-call-service *ngIf="callFeatureEnable && callFeatureEnableForUser"></agora-call-service>
                    <button id="login-button" class="btn btn-outline-success my-2 my-sm-0" *ngIf="!isLoggedIn" (click)="login()">Se connecter</button>
                    <button id="logout-button" class="btn btn-outline-success my-2 my-sm-0" *ngIf="isLoggedIn" (click)="logout()">Déconnexion</button>
                </span>
            </div>
            </div>
        </nav>
    `,
    standalone: true,
    imports: [NgFor, NgIf, RouterOutlet, RouterLink, RouterLinkActive, AgoraCallServiceComponent]
})
export class SiteNavBarComponent implements OnInit {
  useAuthentication: boolean;
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
    this.useAuthentication = environmentService.authEnable ?? false
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
