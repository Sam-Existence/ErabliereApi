import { Injectable } from "@angular/core";
import { AppUser } from "src/model/appuser";
import { AuthResponse } from "src/model/authresponse";
import { IAuthorisationSerivce } from "./iauthorisation-service";

@Injectable({ providedIn: 'root' })
export class AuthorisationBypassService implements IAuthorisationSerivce {
    login(): Promise<void> {
        return new Promise<void>(() => { });
    }
    isLoggedIn(): Promise<Boolean> {
        return new Promise<Boolean>(() => true);
    }
    completeLogin(): Promise<AppUser> {
        return new Promise<AppUser>(() => new AppUser());
    }
    logout(): void {
        
    }
    completeLogout(): Promise<AuthResponse> {
        return new Promise<AuthResponse>(() => new AuthResponse());
    }
    getAccessToken(): Promise<String | null> {
        return new Promise(() => null);
    }
}