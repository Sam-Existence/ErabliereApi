import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { OAuthConfig } from "src/model/oauthConfig";

@Injectable({ providedIn: 'root' })
export class EnvironmentService {
    apiUrl?: string
    appRoot?: string
    clientId?: string
    tenantId?: string
    stsAuthority?: string
    authEnable?: boolean

    constructor(private _httpClient: HttpClient) {
        
    }

    loadConfig() {
        return this._httpClient.get<OAuthConfig>("/assets/config/oauth-oidc.json").toPromise().then(c => 
            {
                this.apiUrl = c.apiUrl;
                this.appRoot = c.appRoot;
                this.clientId = c.clientId;
                this.tenantId = c.tenantId;
                this.stsAuthority = c.stsAuthority;
                this.authEnable = c.authEnable;
            })
            .catch((err: any) => {
                console.error(err);
            });;
    }
}