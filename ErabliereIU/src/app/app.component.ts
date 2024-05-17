import {Component, OnInit} from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { SiteNavBarComponent } from 'src/dashboard/site-nav-bar.component';
import { YouAreNotConnectedComponent } from 'src/dashboard/you-are-note-connected.component';
import { ErabliereSideBarComponent } from 'src/dashboard/erablieres-side-bar.component';
import { NgIf} from '@angular/common';
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
    SiteNavBarComponent,
    YouAreNotConnectedComponent,
    ErabliereSideBarComponent,
    ErabliereAIComponent,
    NgIf
  ]
})
export class AppComponent implements OnInit {
  private _pagesSansMenu = ["apropos", "admin"];
  idErabliereSelectionnee?: string;
  showMenu: boolean = true;
  thereIsAtLeastOneErabliere: boolean = false;
  erabliereAIEnable: boolean = false;
  erabliereAIUserRole: boolean = false;
  authService: IAuthorisationSerivce;

  constructor(private api: ErabliereApi, authServiceFactory: AuthorisationFactoryService, private msalService: MsalService, private _router: Router) {
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

    this.showMenu = true;
    let splitUrl = this._router.url.split("/");

    if (splitUrl.length > 1) {
      let page = splitUrl[1];
      this.showMenu = !this._pagesSansMenu.includes(page);
    }

    // get the user role to see if it got the ErabliereAIUser role
    // if so, enable the chat widget
    if (this.authService.type == "AzureAD") {
      this.checkRoleErabliereAI();
      this.authService.loginChanged.subscribe((val) => {
        this.checkRoleErabliereAI();
      });
    }

    // update the idErabliereSelectionnee when the route changes
    this._router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        const url = val.url;
        const urlParts = url.split('/');
        if (urlParts.length >= 3 && urlParts[1] === 'e') {
          this.idErabliereSelectionnee = urlParts[2];
        }

        if (urlParts.length > 1) {
          this.showMenu = !this._pagesSansMenu.includes(urlParts[1]);
        }
      }
    });
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
