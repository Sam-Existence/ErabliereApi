import { Component, OnInit } from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { RouterOutlet } from '@angular/router';
import { SiteNavBarComponent } from 'src/dashboard/site-nav-bar.component';
import { YouAreNotConnectedComponent } from 'src/dashboard/you-are-note-connected.component';
import { ErabliereSideBarComponent } from 'src/dashboard/erablieres-side-bar.component';
import { NgIf } from '@angular/common';
import { Subject } from 'rxjs';
import { ErabliereAIComponent } from 'src/erabliereai/erabliereai-chat.component';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { MsalService } from '@azure/msal-angular';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';

@Component({
  selector: 'app-root',
  template: `
    <div class="d-flex flex-column min-vh-100">
      <header>
        <site-nav-bar 
          [thereIsAtLeastOneErabliereSubject]="thereIsAtLeastOneErabliereSubject" />
        <you-are-not-connected />
      </header>
      <div class="flex-fill">
        <div class="row">
          <div *ngIf="showMenu" class="col-lg-2">
            <aside>
              <erablieres-side-bar 
                [showMenuSubject]="showMenuSubject" 
                [thereIsAtLeastOneErabliereSubject]="thereIsAtLeastOneErabliereSubject" />
            </aside>
          </div>
          <div class="{{ classes }}">
            <main>
              <router-outlet></router-outlet>
              <entra-redirect></entra-redirect>
            </main>
            <app-chat-widget *ngIf="erabliereAIEnable && erabliereAIUserRole"></app-chat-widget>
          </div>
        </div>
      </div>
      <footer class="footer mt-auto py-3">
        <div class="container-fluid">
          <div class="row">
            <div class="col-12">
              <hr />
              <p style="text-align: center;">© 2024 - ErabliereAPI</p>
            </div>
          </div>
        </div>
      </footer>
    </div>
  `,
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
  showMenuSubject: Subject<boolean> = new Subject<boolean>();
  showMenu: boolean = true;
  thereIsAtLeastOneErabliereSubject: Subject<boolean> = new Subject<boolean>();
  thereIsAtLeastOneErabliere: boolean = false;
  classes: string = "col-lg-10 col-md-12";
  erabliereAIEnable: boolean = false;
  erabliereAIUserRole: boolean = false;
  authService: IAuthorisationSerivce;

  constructor(private api: ErabliereApi, authServiceFactory: AuthorisationFactoryService, private msalService: MsalService) {
    this.authService = authServiceFactory.getAuthorisationService();
    this.showMenuSubject.next(true);
    this.showMenuSubject.subscribe((val) => {
      this.showMenu = val;

      if (this.showMenu) {
        this.classes = "col-lg-10 col-md-12";
      }
      else {
        this.classes = "col-12";
      }
    });
    this.thereIsAtLeastOneErabliereSubject.next(false);
    this.thereIsAtLeastOneErabliereSubject.subscribe((val) => {
      this.thereIsAtLeastOneErabliere = val;
    });
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
