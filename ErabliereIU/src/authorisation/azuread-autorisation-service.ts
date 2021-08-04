import { Router } from '@angular/router';
import { BrowserCacheLocation, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { Configuration } from '@azure/msal-browser/dist/config/Configuration';
import { Subject } from 'rxjs';
import { EnvironmentService } from 'src/environments/environment.service';
import { AppUser } from 'src/model/appuser';
import { AuthResponse } from 'src/model/authresponse';
import { IAuthorisationSerivce } from './iauthorisation-service';

export class AzureADAuthorisationService implements IAuthorisationSerivce {
    private _msalInstance: PublicClientApplication;
    private _activeHomeAccountId?: string
    private _loginChangedSubject = new Subject<Boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor(private _environmentService: EnvironmentService, private _router: Router) {
        if (this._environmentService.clientId == undefined) {
            throw new Error("/assets/config/oauth-oidc.json/clientId cannot be null when using AzureAD authentication mode");
        }
        
        const msalConfig: Configuration = {
            auth: {
                clientId: this._environmentService.clientId,
                authority: "https://login.microsoftonline.com/" + this._environmentService.tenantId,
                redirectUri: "/signin-callback",
                postLogoutRedirectUri: "/signout-callback",
                navigateToLoginRequestUrl: true
            },
            cache: {
                cacheLocation: BrowserCacheLocation.LocalStorage,
                secureCookies: true
            },
            system: {
                loggerOptions: {
                    loggerCallback: (level: LogLevel, message: string, containsPii: boolean): void => {
                        if (containsPii) {
                            return;
                        }
                        switch (level) {
                            case LogLevel.Error:
                                console.error(message);
                                return;
                            case LogLevel.Info:
                                console.info(message);
                                return;
                            case LogLevel.Verbose:
                                console.debug(message);
                                return;
                            case LogLevel.Warning:
                                console.warn(message);
                                return;
                        }
                    },
                    piiLoggingEnabled: false
                },
                windowHashTimeout: 60000,
                iframeHashTimeout: 6000,
                loadFrameTimeout: 0,
                asyncPopups: false
            }
        }
        
        this._msalInstance = new PublicClientApplication(msalConfig);
     }
    
     login() {
         console.log("Login")
         return this._msalInstance.loginPopup().then(async response => {
            this._msalInstance.setActiveAccount(response.account);

            this._activeHomeAccountId = response.account?.homeAccountId;

            await this.completeLogin();
         });
     }

     isLoggedIn(): Promise<Boolean> {
        console.log("Is logged in");
        return new Promise(async (resolve, reject) => {
            const user = this._msalInstance.getActiveAccount();

            const isLoggedIn = !!user && user.homeAccountId == this._activeHomeAccountId;

            if (this._activeHomeAccountId !== user?.localAccountId) {
                this._loginChangedSubject.next(isLoggedIn);
            }

            console.debug("Is logged in result: " + isLoggedIn);

            return resolve(isLoggedIn);
        });
     }

    completeLogin() {
        console.log("Complete login")
        return new Promise<AppUser>((resolve, reject) => {
            const user = this._msalInstance.getActiveAccount();

            this._loginChangedSubject.next(!!user);

            if (user != null) {
                this._activeHomeAccountId = user.homeAccountId;
            }
            
            console.debug("Home account id: " + this._activeHomeAccountId);

            return resolve(new AppUser());
        });
    }

    logout() {
        console.log("Logout")
        this._msalInstance.loginPopup().then(async response => {
            await this.completeLogout();
        });
    }

    completeLogout() {
        return new Promise<AuthResponse>((resolve, reject) => {
            console.debug("Complete logout");
            this._activeHomeAccountId = undefined;
            this._loginChangedSubject.next(false);
            return resolve(new AuthResponse());
        })
    }

    getAccessToken() : Promise<String | null> {
        console.debug("Get access token");
        console.debug(this._activeHomeAccountId);
        if (this._activeHomeAccountId == null) {
            const user = this._msalInstance.getActiveAccount();

            this._loginChangedSubject.next(!!user);

            if (user != null) {
                this._activeHomeAccountId = user.homeAccountId;
            }
            else {
                console.debug("Active account is null, return null.");
                return new Promise((resolve, reject) => resolve(null));
            }
        }

        const requestObj = {
            scopes: this._environmentService.scopes?.split(' ') ?? []
        };

        console.debug("acquireTokenSilent.");
        return this._msalInstance.acquireTokenSilent(requestObj).then(user => {
            console.debug(user);
            if (!!user && !!user.accessToken) {
                console.debug("Return access token succesfully");
                return user.accessToken;
            }
            else {
                console.debug("Return access token unsuccesfully (null)");
                return null;
            }
        });
    }
}