import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationService } from '../authorisation-service';

@Component({
    selector: 'app-signin-callback',
    template: '<div></div>'
})

export class SigninRedirectCallbackComponent implements OnInit {
    constructor(private _authService: AuthorisationService,
                private _router: Router) { }

    ngOnInit() {
        this._authService.completeLogin().then(user => {
            this._router.navigate(['/'], {replaceUrl: true});
        });
     }
}