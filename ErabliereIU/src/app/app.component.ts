import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bd-navbar">
      <div class="container-fluid">
        <h2 class="mr-5">{{ title }}</h2>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="navbar-collapse collapse d-lg-inline-flex flex-lg-row">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0">
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
        </div>
      </div>
    </nav>
    <erablieres [pageSelectionnee]="pageSelectionnee" [cacheMenuErabliere]="cacheMenuErabliere"></erablieres>
  `
})
export class AppComponent {
  title = 'Érablière IU';
  pageSelectionnee = 0;
  cacheMenuErabliere = false;

  selectionnerPage(i:number) {
    this.pageSelectionnee = i;
    this.cacheMenuErabliere = i == 3 || i == 4;
  }
}
