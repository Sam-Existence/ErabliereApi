import { Component } from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { RouterOutlet } from '@angular/router';
import { SiteNavBarComponent } from 'src/dashboard/site-nav-bar.component';
import { YouAreNotConnectedComponent } from 'src/dashboard/you-are-note-connected.component';
import { ErabliereSideBarComponent } from 'src/dashboard/erablieres-side-bar.component';
import { NgIf } from '@angular/common';
import { EventEmitter } from 'stream';
import { Subject } from 'rxjs';

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
          <div *ngIf="showMenu" class="col-lg-2">
            <aside>
              <erablieres-side-bar [showMenuSubject]="showMenuSubject"></erablieres-side-bar>
            </aside>
          </div>
          <div class="{{ classes }}">
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
    ErabliereSideBarComponent,
    NgIf
  ]
})
export class AppComponent {
  showMenuSubject: Subject<boolean> = new Subject<boolean>();
  showMenu: boolean = true;
  classes: string = "col-lg-10 col-md-12";

  constructor() {
    this.showMenuSubject.next(true);
    this.showMenuSubject.subscribe((val) => {
      this.showMenu = val;

      if (this.showMenu) {
        this.classes = "col-lg-10 col-md-12";
      }
      else {
        this.classes = "col-lg-12 col-md-12";
      }
    });
  }
}
