import { Injectable } from '@angular/core';
import { UserManager, User, UserManagerSettings } from 'oidc-client'
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthorisationService {
    private _userManager: UserManager;
    private _user?: User | null;
    private _loginChangedSubject = new Subject<Boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor() {
        const stsSettings:UserManagerSettings = {
            authority: environment.stsAuthority,
            client_id: environment.clientId,
            client_secret: "secret",
            redirect_uri: `${environment.appRoot}/signin-callback`,
            scope: "openid profile erabliereapi",
            response_type: 'code',
            post_logout_redirect_uri: `${environment.stsAuthority}/signout-callback`
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