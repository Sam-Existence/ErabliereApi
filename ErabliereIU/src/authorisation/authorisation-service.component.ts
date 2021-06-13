import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client'
import { Subject } from 'rxjs';
import { EnvironmentService } from 'src/environments/environment.service';

@Injectable({ providedIn: 'root' })
export class AuthorisationService {
    private _userManager: UserManager;
    private _user?: User | null;
    private _loginChangedSubject = new Subject<Boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor(private _environmentService: EnvironmentService) {
        const stsSettings:UserManagerSettings = {
            authority: _environmentService.stsAuthority,
            client_id: _environmentService.clientId,
            client_secret: "secret",
            redirect_uri: `${_environmentService.appRoot}/signin-callback`,
            scope: "openid profile erabliereapi",
            response_type: 'code',
            post_logout_redirect_uri: `${_environmentService.stsAuthority}/signout-callback`
        };
        this._userManager = new UserManager(stsSettings);
     }
    
     login() {
         return this._userManager.signinRedirect();
     }

     isLoggedIn(): Promise<Boolean> {
         return this._userManager.getUser().then(user => {
            const isLoggedIn = !!user && !user.expired;
            if (this._user !== user) {
                this._loginChangedSubject.next(isLoggedIn);
            }
            this._user = user;

            return isLoggedIn;
         });
     }

    completeLogin() {
        return this._userManager.signinRedirectCallback().then(user => {
            this._user = user;
            this._loginChangedSubject.next(!!user && !user.expired);
            return user;
        });
    }

    logout() {
        this._userManager.signoutRedirect();
    }

    completeLogout() {
        this._user = null;
        return this._userManager.signoutRedirectCallback();
    }

    getAccessToken() : Promise<String | null> {
        return this._userManager.getUser().then(user => {
            if (!!user && !user.expired) {
                return user.access_token;
            }
            else {
                return null;
            }
        });
    }
}