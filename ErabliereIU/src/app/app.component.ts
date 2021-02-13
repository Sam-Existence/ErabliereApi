import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <header class="navbar navbar-expand navbar-light flex-column flex-md-row bd-navbar">
      <h2 class="mr-5">{{ title }}</h2>
      <div class="navbar-nav-scroll">
        <ul class="navbar-nav bd-navbar-nav flex-row">
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
    </header>
    <erablieres [pageSelectionnee]="pageSelectionnee"></erablieres>
  `
})
export class AppComponent {
  title = 'Erabliere IU';
  pageSelectionnee = 0;

  selectionnerPage(i:number) {
    this.pageSelectionnee = i;
  }
}
