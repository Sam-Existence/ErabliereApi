import { MsalService } from '@azure/msal-angular';
import { AccountInfo, PopupRequest, SilentRequest } from '@azure/msal-browser';
import { Subject } from 'rxjs';
import { EnvironmentService } from 'src/environments/environment.service';
import { AppUser } from 'src/model/appuser';
import { AuthResponse } from 'src/model/authresponse';
import { IAuthorisationSerivce } from './iauthorisation-service';

export class AzureADAuthorisationService implements IAuthorisationSerivce {
  private _isLoggedIn: boolean = false;
  private _loginChangedSubject = new Subject<boolean>();
  loginChanged = this._loginChangedSubject.asObservable();
  type: string = "AzureAD";
  initialize: boolean = false;

  constructor(private _msalInstance: MsalService, private _environmentService: EnvironmentService) { }

  async login() {
    console.log("login");
    if (this.initialize == false) {
      console.log("Initilize MSAL Instance");
      await this._msalInstance.initialize().toPromise();
      this.initialize = true;
    }
    else {
      console.log("MSAL already initialize at login");
    }
    const popupParam: PopupRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      prompt: "select_account"
    }
    var appUser = await this._msalInstance.loginPopup(popupParam).toPromise().then(response => {
      return this.completeLogin();
    });
    console.log("AppUser", appUser);
  }

  async isLoggedIn(): Promise<boolean> {
    console.log("isLoggedIn");
    if (this.initialize == false) {
      console.log("Initilize MSAL Instance");
      await this._msalInstance.initialize().toPromise();
      this.initialize = true;
    }
    else {
      console.log("MSAL already initialize at isLoggedIn");
    }
    
    let user = this.getUser();

    this._isLoggedIn = user != null;

    return this._isLoggedIn;
  }

  async completeLogin(): Promise<AppUser> {
    console.log("completeLogin")
    if (this.initialize == false) {
      console.log("Initilize MSAL Instance");
      await this._msalInstance.initialize().toPromise();
      this.initialize = true;
    }
    else {
      console.log("MSAL already initialize at completeLogin");
    }
    
    const user = this.getUser();
    if (user != null) {
      this._isLoggedIn = true;
      this._loginChangedSubject.next(true);
      this._msalInstance.instance.setActiveAccount(user);
      return new AppUser();
    }

    this._isLoggedIn = false;
    this._loginChangedSubject.next(false);
    this._msalInstance.instance.setActiveAccount(null);
    return new AppUser();
  }

  logout() {
    this._msalInstance.logoutPopup().subscribe(async response => {
      await this.completeLogout();
    });
  }

  completeLogout(): Promise<AuthResponse> {
    return new Promise<AuthResponse>((resolve, reject) => {
      this._isLoggedIn = false;
      this._msalInstance.instance.setActiveAccount(null);
      this._loginChangedSubject.next(false);
      return resolve(new AuthResponse());
    })
  }

  async getAccessToken(): Promise<String | null> {
    console.log("getAccessToken");
    if (this.initialize == false) {
      console.log("Initilize MSAL Instance");
      await this._msalInstance.initialize().toPromise();
      this.initialize = true;
    }
    else {
      console.log("MSAL already initialize at getAccessToken");
    }
    
    var user = this.getUser();
    this._msalInstance.instance.setActiveAccount(user);

    const requestObj: SilentRequest = {
      scopes: this._environmentService.scopes?.split(' ') ?? [],
      authority: this._environmentService.stsAuthority,
      account: this.getUser() ?? undefined,
      forceRefresh: false
    };

    return this._msalInstance.acquireTokenSilent(requestObj).toPromise().then(authResult => {
      if (this._isLoggedIn == false) {
        this.completeLogin();
      }
      return authResult?.accessToken ?? null;
    }).catch(reason => {
      console.log(reason);
      this._isLoggedIn = false;
      this._msalInstance.instance.setActiveAccount(null);
      this._loginChangedSubject.next(false);
      return null;
    });
  }

  getUser(): AccountInfo | null {
    var user = null;
    if (this._msalInstance.instance.getAllAccounts().length > 0) {
      user = this._msalInstance.instance.getAllAccounts()[0];
    }
    return user;
  }
}
