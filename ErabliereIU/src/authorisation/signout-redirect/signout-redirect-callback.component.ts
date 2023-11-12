import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorisationFactoryService } from '../authorisation-factory-service';
import { IAuthorisationSerivce } from '../iauthorisation-service';

@Component({
    selector: 'signout-redirect-callback',
    template: '<div></div>',
    standalone: true
})

export class SignoutRedirectCallbackComponent implements OnInit {
    private _authService: IAuthorisationSerivce

    constructor(private _authFactoryService: AuthorisationFactoryService,
                private _router: Router) 
                {
                    this._authService = _authFactoryService.getAuthorisationService();
                }

    ngOnInit() { 
        this._authService.completeLogout().then(_ => {
            this._router.navigate(['/'], { replaceUrl: true });
        })
    }
}