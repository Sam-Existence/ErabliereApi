import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from '../environments/environment.service';
import { UrlModel } from '../model/urlModel';
import { NgFor, NgIf } from '@angular/common';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
    selector: 'site-nav-bar',
    templateUrl: 'site-nav-bar.component.html',
    standalone: true,
    imports: [NgFor, NgIf, RouterOutlet, RouterLink, RouterLinkActive]
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

  constructor(
      authFactoryService: AuthorisationFactoryService, 
      private environmentService: EnvironmentService, 
      private cdr: ChangeDetectorRef,
      private router: Router) {
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
  }

  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }
}
