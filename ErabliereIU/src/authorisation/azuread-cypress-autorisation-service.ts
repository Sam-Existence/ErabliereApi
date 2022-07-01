import { Subject } from 'rxjs';
import { AppUser } from 'src/model/appuser';
import { AuthResponse } from 'src/model/authresponse';
import { IAuthorisationSerivce } from './iauthorisation-service';

export class AzureADCypressAuthorisationService implements IAuthorisationSerivce {
  private _isLoggingIn = Boolean();
  private _loginChangedSubject = new Subject<Boolean>();
  type: string = "AzureADCypress";
  loginChanged = this._loginChangedSubject.asObservable();

  async login() {
    var appUser = await this.completeLogin();
    console.log(appUser);
  }

  isLoggedIn(): Promise<Boolean> {
    return new Promise<Boolean>((resolve, reject) => {
      if (this.getAccessToken() != null && this._isLoggingIn == false) {
        this._isLoggingIn = true;
        this._loginChangedSubject.next(true);
      }

      return resolve(this._isLoggingIn);
    });
  }

  completeLogin() {
    return new Promise<AppUser>((resolve, reject) => {
      if (this._isLoggingIn == false) {
        this._isLoggingIn = true;
        this._loginChangedSubject.next(true);
      }
      return resolve(new AppUser());
    });
  }

  logout() {
    this.completeLogout();
  }

  completeLogout() {
    return new Promise<AuthResponse>((resolve, reject) => {
      this._isLoggingIn = false;
      this._loginChangedSubject.next(false);
      return resolve(new AuthResponse());
    });
  }

  getAccessToken(): Promise<String | null> {
    return new Promise<String | null>((resolve, reject) => {
        var token = localStorage.getItem("adal.idtoken");

        console.log(token);

        if (token != null && this._isLoggingIn == false) {
          this._isLoggingIn = true;
          this._loginChangedSubject.next(true);
        }

        return resolve(token);
    });
  }
}
