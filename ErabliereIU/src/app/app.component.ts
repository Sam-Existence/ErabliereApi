import { Component } from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { RouterOutlet } from '@angular/router';
import { SiteNavBarComponent } from 'src/dashboard/site-nav-bar.component';
import { YouAreNotConnectedComponent } from 'src/dashboard/you-are-note-connected.component';
import { ErabliereSideBarComponent } from 'src/dashboard/erablieres-side-bar.component';

@Component({
  selector: 'app-root',
  template: `
    <div class="d-flex flex-column min-vh-100">
      <header>
        <site-nav-bar></site-nav-bar>
        <you-are-not-connected></you-are-not-connected>
      </header>
      <div class="flex-fill">
        <div class="row">
          <div class="col-lg-2">
            <aside>
              <erablieres-side-bar></erablieres-side-bar>
            </aside>
          </div>
          <div class="col-lg-10 col-md-12">
            <main>
              <router-outlet></router-outlet>
              <entra-redirect></entra-redirect>
            </main>
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
    ErabliereSideBarComponent
  ]
})
export class AppComponent {

}
