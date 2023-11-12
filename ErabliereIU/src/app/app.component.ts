import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div>
      <router-outlet></router-outlet>
      <entra-redirect></entra-redirect>
    </div>
  `
})
export class AppComponent {

}
