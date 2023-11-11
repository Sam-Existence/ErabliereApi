import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div>
      <router-outlet></router-outlet>
      <app-redirect></app-redirect>
    </div>
  `
})
export class AppComponent {

}
