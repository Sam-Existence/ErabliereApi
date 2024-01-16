import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { EnvironmentService } from '../environments/environment.service';
import { ErabliereComponent as ErabliereComponent_1 } from '../erablieres/erabliere.component';
import { NgFor, NgIf } from '@angular/common';

@Component({
    selector: 'you-are-not-connected',
    template: `
        <div *ngIf="!isLoggedIn" class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <div class="alert alert-information" role="alert">
                        <strong>Vous n'êtes pas connecté </strong> <a href="#" (click)="login()">Cliquer ici pour vous connecter</a>
                        <p *ngIf="tenantId == 'common'">Connectez-vous maintenant avec votre compte Microsoft.</p>
                        <p>Pour obtenir un compte, communiquer à l'administrateur. Vous trouverez les informations dans la page À propos.</p>
                    </div>
                </div>
            </div>
        </div>
    `,
    standalone: true,
    imports: [NgFor, NgIf, ErabliereComponent_1]
})
export class YouAreNotConnectedComponent implements OnInit {
  cacheMenuErabliere: boolean;
  thereIsAtLeastOneErabliere: boolean;
  useAuthentication: boolean;
  isLoggedIn: boolean;
  tenantId?: string;

  private _authService: IAuthorisationSerivce

  constructor(
      authFactoryService: AuthorisationFactoryService, 
      private environmentService: EnvironmentService, 
      private cdr: ChangeDetectorRef) {
    this.cacheMenuErabliere = false
    this._authService = authFactoryService.getAuthorisationService()
    this.useAuthentication = environmentService.authEnable ?? false
    this.thereIsAtLeastOneErabliere = false
    this.isLoggedIn = false
  }
  ngOnInit(): void {
    this._authService.isLoggedIn().then(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
    this._authService.loginChanged.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      this.cdr.detectChanges();
    });
    this.tenantId = this.environmentService.tenantId;
  }

  login() {
    this._authService.login();
  }
}
