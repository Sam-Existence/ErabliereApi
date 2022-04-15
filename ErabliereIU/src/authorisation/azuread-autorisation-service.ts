import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { BrowserCacheLocation, LogLevel, PopupRequest, PublicClientApplication, SilentRequest } from '@azure/msal-browser';
import { Configuration } from '@azure/msal-browser/dist/config/Configuration';
import { Subject } from 'rxjs';
import { EnvironmentService } from 'src/environments/environment.service';
import { AppUser } from 'src/model/appuser';
import { AuthResponse } from 'src/model/authresponse';
import { IAuthorisationSerivce } from './iauthorisation-service';

export class AzureADAuthorisationService implements IAuthorisationSerivce {
  private _activeHomeAccountId?: string
  private _loginChangedSubject = new Subject<Boolean>();

  loginChanged = this._loginChangedSubject.asObservable();

  constructor(private _msalInstance: MsalService, private _environmentService: EnvironmentService) {
    
  }

  async login() {
    const popupParam: PopupRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      prompt: "select_account"
    }
    const response = await this._msalInstance.loginPopup(popupParam);
    await this.completeLogin();
  }

  isLoggedIn(): Promise<Boolean> {
    return new Promise(async (resolve, reject) => {
      let user = this._msalInstance.instance.getActiveAccount();

      if (user == null) {
        const users = this._msalInstance.instance.getAllAccounts();

        if (users.length > 0) {
          user = users[0];
          this._activeHomeAccountId = user.homeAccountId
          this._msalInstance.instance.setActiveAccount(user);
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
      const user = this._msalInstance.instance.getActiveAccount();

      this._loginChangedSubject.next(!!user);

      if (user != null) {
        this._activeHomeAccountId = user.homeAccountId;
      }

      return resolve(new AppUser());
    });
  }

  logout() {
    this._msalInstance.loginPopup().subscribe(async response => {
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
      const user = this._msalInstance.instance.getActiveAccount();

      this._loginChangedSubject.next(!!user);

      if (user != null) {
        this._activeHomeAccountId = user.homeAccountId;
      }
      else {
        return new Promise((resolve, reject) => resolve(null));
      }
    }

    const accountInfo = this._msalInstance.instance.getAccountByHomeId(this._activeHomeAccountId) ?? undefined;

    const requestObj: SilentRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      authority: this._environmentService.stsAuthority,
      account: accountInfo,
      forceRefresh: false
    };

    return this._msalInstance.acquireTokenSilent(requestObj).toPromise().then(authResult => {
      return authResult.accessToken;
    }).catch(reason => {
      console.log(reason);
      this._activeHomeAccountId = undefined;
      this._msalInstance.instance.setActiveAccount(null);
      this._loginChangedSubject.next(false);
      return null;
    });
  }
}
