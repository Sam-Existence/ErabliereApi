import { AppUser } from "src/model/appuser";
import { AuthResponse } from "src/model/authresponse";

export interface IAuthorisationSerivce {
    login(): Promise<void>
    isLoggedIn(): Promise<Boolean>
    completeLogin(): Promise<AppUser>
    logout(): void
    completeLogout(): Promise<AuthResponse>
    getAccessToken() : Promise<String | null>
}