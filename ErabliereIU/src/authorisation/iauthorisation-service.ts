import { Observable } from "rxjs";
import { AppUser } from "src/model/appuser";
import { AuthResponse } from "src/model/authresponse";

export interface IAuthorisationSerivce {
    type: string;
    loginChanged:Observable<boolean>
    login(): Promise<void>
    isLoggedIn(): Promise<boolean>
    completeLogin(): Promise<AppUser>
    logout(): void
    completeLogout(): Promise<AuthResponse>
    getAccessToken() : Promise<String | null>
}