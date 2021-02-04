import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <header class="navbar navbar-expand navbar-dark flex-column flex-md-row bd-navbar">
      <h2>Érablière IU</h2>
    </header>
    <menu-erabliere></menu-erabliere>
  `
})
export class AppComponent {
  title = 'ErabliereIU';
}
