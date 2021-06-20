import { Component, OnInit } from '@angular/core';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { IAuthorisationSerivce } from 'src/authorisation/iauthorisation-service';
import { environment } from 'src/environments/environment';

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
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 1" (click)="selectionnerPage(1)" role="button">Alerte</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 2" (click)="selectionnerPage(2)" role="button">Camera</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 4" (click)="selectionnerPage(4)" role="button">Documentation</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" [class.active]="pageSelectionnee === 3" (click)="selectionnerPage(3)" role="button">À propos</a>
                    </li>
                </ul>
                <span [hidden]="useAuthentication == false">
                    <button class="btn btn-outline-success my-2 my-sm-0" *ngIf="!isLoggedIn" (click)="login()">Se connecter</button>
                    <button class="btn btn-outline-success my-2 my-sm-0" *ngIf="isLoggedIn" (click)="logout()">Déconnexion</button>
                </span>
            </div>
            </div>
        </nav>
        <erablieres [pageSelectionnee]="pageSelectionnee" [cacheMenuErabliere]="cacheMenuErabliere"></erablieres>
    `
})
export class DashboardComponent implements OnInit {
    pageSelectionnee = 0;
    cacheMenuErabliere = false;
    title = 'Érablière IU';

    isLoggedIn: Boolean = false;
    useAuthentication: Boolean = environment.enableAuth

    private _authService: IAuthorisationSerivce

    constructor(private _authFactoryService: AuthorisationFactoryService) {
        this._authService = _authFactoryService.getAuthorisationService();
        this._authService.loginChanged.subscribe(loggedIn => {
            this.isLoggedIn = loggedIn;
        })
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

    selectionnerPage(i: number) {
        this.pageSelectionnee = i;
        this.cacheMenuErabliere = i == 3 || i == 4;

        if (this.pageSelectionnee == 1) {
            // Should call Alerte api
        }
    }
}