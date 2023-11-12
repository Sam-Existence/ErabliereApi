import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationFactoryService } from '../authorisation-factory-service';
import { IAuthorisationSerivce } from '../iauthorisation-service';

@Component({
    selector: 'app-signin-callback',
    template: '<div></div>',
    standalone: true
})

export class SigninRedirectCallbackComponent implements OnInit {
    private _authService: IAuthorisationSerivce

    constructor(private _authFactoryService: AuthorisationFactoryService, private _router: Router) 
    {
        this._authService = _authFactoryService.getAuthorisationService();
    }

    async ngOnInit() {
        await this._authService.completeLogin().then(user => {
            this._router.navigate(['/'], {replaceUrl: true});
        });
    }
}