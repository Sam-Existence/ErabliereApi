import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { environment } from 'src/environments/environment';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { EnvironmentService } from '../environments/environment.service';
import { UrlModel } from '../model/urlModel';

@Component({
  selector: 'dashboard',
  template: `
        <nav class="navbar navbar-expand-lg navbar-light bd-navbar">
            <div class="container-fluid">
            <h2 class="mr-5">{{ title }}</h2>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-lg-inline-flex flex-lg-row">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0 mr-auto">
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 0" (click)="selectionnerPage(0)" role="button">Graphique</a>
                    </li>
                    <li *ngFor="let url of urls" *ngIf="url.requiredLogin == false || isLoggedIn" class="nav-item">
                        <a class="nav-link" href="{{ url.href }}" role="button">{{ url.text }}</a>
                    </li>
                    <li *ngIf="isLoggedIn" class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 1" (click)="selectionnerPage(1)" role="button">Alerte</a>
                    </li>
                    <li *ngIf="isLoggedIn" class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 5" (click)="selectionnerPage(5)" role="button">Notes</a>
                    </li>
                    <li *ngIf="isLoggedIn" class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 4" (click)="selectionnerPage(4)" role="button">Documentation</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 3" (click)="selectionnerPage(3)" role="button">À propos</a>
                    </li>
                </ul>
                <span [hidden]="!useAuthentication">
                    <button id="login-button" class="btn btn-outline-success my-2 my-sm-0" *ngIf="!isLoggedIn" (click)="login()">Se connecter</button>
                    <button id="logout-button" class="btn btn-outline-success my-2 my-sm-0" *ngIf="isLoggedIn" (click)="logout()">Déconnexion</button>
                </span>
            </div>
            </div>
        </nav>
        <div *ngIf="!isLoggedIn" class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <div class="alert alert-information" role="alert">
                        <strong>Vous n'êtes pas connecté </strong> <a href="#" (click)="login()">Cliquer ici pour vous connecter</a>
                        <p>Pour obtenir un compte, communiquer à l'administrateur. Vous trouverez les informations dans la page À propos.</p>
                    </div>
                </div>
            </div>
        </div>
        <erablieres [pageSelectionnee]="pageSelectionnee" [cacheMenuErabliere]="cacheMenuErabliere" #erabliereComponent></erablieres>
    `
})
export class DashboardComponent implements OnInit {
  pageSelectionnee = 0;
  cacheMenuErabliere = false;
  title = 'Érablière IU';

  isLoggedIn: Boolean = false;
  useAuthentication: Boolean = environment.enableAuth;
  urls: UrlModel[] = [];

  private _authService: IAuthorisationSerivce

  constructor(authFactoryService: AuthorisationFactoryService, environmentService: EnvironmentService) {
    this._authService = authFactoryService.getAuthorisationService();
    this._authService.loginChanged.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
    this.urls = environmentService.additionnalUrls ?? [];
  }

  ngOnInit(): void {
    this._authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  login() {
    this._authService.login();
  }

  logout() {
    this._authService.logout();
  }

  @ViewChild('erabliereComponent') erabierePage?: ErabliereComponent

  selectionnerPage(i: number) {
    this.pageSelectionnee = i;
    this.cacheMenuErabliere = i == 3;

    if (this.pageSelectionnee == 1) {
      this.erabierePage?.loadAlertes();
    }

    if (this.pageSelectionnee == 4) {
      this.erabierePage?.loadDocumentations();
    }

    if (this.pageSelectionnee == 5) {
      this.erabierePage?.loadNotes();
    }
  }
}
