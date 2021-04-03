import { Injectable } from '@angular/core';
import { AppModule } from 'src/app/app.module';
import { UserManager, User } from 'oidc-client'
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';

@Injectable()
export class AuthorisationService {
    private _userManager: UserManager;
    private _user?: User | null;
    private _loginChangedSubject = new Subject<Boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor() {
        const stsSettings = {
            authority: environment.stsAuthority,
            client_id: environment.clientId,
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
            const currentUser = !!user && !user.expired;
            if (this._user !== user) {
                this._loginChangedSubject.next(currentUser);
            }
            this._user = user;
            return currentUser;
         })
     }
}