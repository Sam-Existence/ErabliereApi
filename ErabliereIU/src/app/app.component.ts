import { Component } from '@angular/core';
import { EntraRedirectComponent } from './entra-redirect.component';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-root',
    template: `
    <div>
      <router-outlet></router-outlet>
      <entra-redirect></entra-redirect>
    </div>
  `,
    standalone: true,
    imports: [RouterOutlet, EntraRedirectComponent]
})
export class AppComponent {

}
