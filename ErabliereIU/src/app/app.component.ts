import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <header class="navbar navbar-expand navbar-light flex-column flex-md-row bd-navbar">
      <h2 class="mr-5">{{ title }}</h2>
      <div class="navbar-nav-scroll">
        <ul class="navbar-nav bd-navbar-nav flex-row">
          <li class="nav-item">
            <a class="nav-link active">Graphique</a>
          </li>
          <li class="nav-item">
            <a class="nav-link">Alerte</a>
          </li>
          <li class="nav-item">
            <a class="nav-link">Camera</a>
          </li>
          <li class="nav-item">
            <a class="nav-link">À propos</a>
          </li>
        </ul>
      </div>
    </header>
    <erablieres></erablieres>
  `
})
export class AppComponent {
  title = 'Erabliere IU';
}
