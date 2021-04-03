import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationService } from '../authorisation-service.component';

@Component({
    selector: 'signout-redirect-callback',
    template: '<div></div>'
})

export class SignoutRedirectCallbackComponent implements OnInit {
    constructor(private _authService: AuthorisationService,
                private _router: Router) { }

    ngOnInit() { 
        this._authService.completeLogout().then(_ => {
            this._router.navigate(['/'], { replaceUrl: true });
        })
    }
}