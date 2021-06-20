import { Injectable } from '@angular/core';
import { LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { Configuration } from '@azure/msal-browser/dist/config/Configuration';
import { exception } from 'console';
import { Subject } from 'rxjs';
import { EnvironmentService } from 'src/environments/environment.service';
import { AppUser } from 'src/model/appuser';
import { AuthResponse } from 'src/model/authresponse';
import { IAuthorisationSerivce } from './iauthorisation-service';

@Injectable({ providedIn: 'root' })
export class AzureADAuthorisationService implements IAuthorisationSerivce {
    private _msalInstance: PublicClientApplication;
    private _activeAcoountId?: string
    private _loginChangedSubject = new Subject<Boolean>();

    loginChanged = this._loginChangedSubject.asObservable();

    constructor(private _environmentService: EnvironmentService) {
        if (this._environmentService.clientId == undefined) {
            throw exception("/assets/config/oauth-oidc.json/clientId cannot be null when using AzureAD authentication mode");
        }
        
        const msalConfig: Configuration = {
            auth: {
                clientId: this._environmentService.clientId,
                authority: "https://login.microsoftonline.com/" + this._environmentService.tenantId,
                knownAuthorities: [],
                cloudDiscoveryMetadata: "",
                redirectUri: "/signin-callback",
                postLogoutRedirectUri: "/signout-callback",
                navigateToLoginRequestUrl: true,
                clientCapabilities: ["CP1"]
            },
            cache: {
                cacheLocation: "sessionStorage",
                storeAuthStateInCookie: false,
                secureCookies: false
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
         return this._msalInstance.loginRedirect();
     }

     isLoggedIn(): Promise<Boolean> {
        return new Promise(() => {
            const user = this._msalInstance.getActiveAccount();

            const isLoggedIn = !!user && user.tenantId == this._environmentService.tenantId;

            if (this._activeAcoountId !== user?.localAccountId) {
                this._loginChangedSubject.next(isLoggedIn);
            }

            return isLoggedIn;
        });
     }

    completeLogin() {
        return new Promise<AppUser>(() => {
            const user = this._msalInstance.getActiveAccount();
            this._loginChangedSubject.next(!!user);
            this._activeAcoountId = user?.homeAccountId;
            // return user;
            return new AppUser();
        });
    }

    logout() {
        this._msalInstance.loginRedirect();
    }

    completeLogout() {
        return new Promise<AuthResponse>(() => {
            this._activeAcoountId = undefined;
            return new AuthResponse();
        });
    }

    getAccessToken() : Promise<String | null> {
        const requestObj = {
            scopes: ["user.read"]
        };

        return this._msalInstance.acquireTokenSilent(requestObj).then(user => {
            if (!!user && !!user.accessToken) {
                return user.accessToken;
            }
            else {
                return null;
            }
        });
    }
}