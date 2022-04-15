import { Router } from '@angular/router';
import { BrowserCacheLocation, LogLevel, PopupRequest, PublicClientApplication, SilentRequest } from '@azure/msal-browser';
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

  constructor(private _environmentService: EnvironmentService) {
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
        storeAuthStateInCookie: false,
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

  async login() {
    const popupParam: PopupRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      prompt: "select_account"
    }
    const response = await this._msalInstance.loginPopup(popupParam);
    this._msalInstance.setActiveAccount(response.account);
    this._activeHomeAccountId = response.account?.homeAccountId;
    await this.completeLogin();
  }

  isLoggedIn(): Promise<Boolean> {
    return new Promise(async (resolve, reject) => {
      let user = this._msalInstance.getActiveAccount();

      if (user == null) {
        const users = this._msalInstance.getAllAccounts();

        if (users.length > 0) {
          user = users[0];
          this._activeHomeAccountId = user.homeAccountId
          this._msalInstance.setActiveAccount(user);
        }
      }

      const isLoggedIn = !!user && user.homeAccountId == this._activeHomeAccountId;

      if (this._activeHomeAccountId !== user?.localAccountId) {
        this._loginChangedSubject.next(isLoggedIn);
      }

      return resolve(isLoggedIn);
    });
  }

  completeLogin() {
    return new Promise<AppUser>((resolve, reject) => {
      const user = this._msalInstance.getActiveAccount();

      this._loginChangedSubject.next(!!user);

      if (user != null) {
        this._activeHomeAccountId = user.homeAccountId;
      }

      return resolve(new AppUser());
    });
  }

  logout() {
    this._msalInstance.loginPopup().then(async response => {
      await this.completeLogout();
    });
  }

  completeLogout() {
    return new Promise<AuthResponse>((resolve, reject) => {
      this._activeHomeAccountId = undefined;
      this._loginChangedSubject.next(false);
      return resolve(new AuthResponse());
    })
  }

  getAccessToken(): Promise<String | null> {
    if (this._activeHomeAccountId == null) {
      const user = this._msalInstance.getActiveAccount();

      this._loginChangedSubject.next(!!user);

      if (user != null) {
        this._activeHomeAccountId = user.homeAccountId;
      }
      else {
        return new Promise((resolve, reject) => resolve(null));
      }
    }

    const accountInfo = this._msalInstance.getAccountByHomeId(this._activeHomeAccountId) ?? undefined;

    const requestObj: SilentRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      authority: this._environmentService.stsAuthority,
      account: accountInfo
    };

    return this._msalInstance.acquireTokenSilent(requestObj).then(user => {
      if (!!user && !!user.accessToken) {
        return user.accessToken;
      }
      else {
        return null;
      }
    })
      .catch(reason => {
        console.log(reason);
        this._activeHomeAccountId = undefined;
        this._msalInstance.setActiveAccount(null);
        this._loginChangedSubject.next(false);
        return null;
      });
  }
}
