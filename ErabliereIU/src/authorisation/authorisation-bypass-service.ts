import { Observable } from "rxjs";
import { AppUser } from "src/model/appuser";
import { AuthResponse } from "src/model/authresponse";
import { IAuthorisationSerivce } from "./iauthorisation-service";

export class AuthorisationBypassService implements IAuthorisationSerivce {
    loginChanged: Observable<Boolean> = new Observable<Boolean>();

    login(): Promise<void> {
        return new Promise<void>((resolve, reject) => { return resolve(); });
    }
    isLoggedIn(): Promise<Boolean> {
        return new Promise<Boolean>((resolve, reject) => resolve(true));
    }
    completeLogin(): Promise<AppUser> {
        return new Promise<AppUser>((resolve, reject) => resolve(new AppUser()));
    }
    logout(): void {
        
    }
    completeLogout(): Promise<AuthResponse> {
        return new Promise<AuthResponse>((resolve, reject) => resolve(new AuthResponse()));
    }
    getAccessToken(): Promise<String | null> {
        console.log("getAccessToken");
        return new Promise((resolve, reject) => {
            console.log("getAccessTokenPromise");
            return resolve(null);
        });
    }
}