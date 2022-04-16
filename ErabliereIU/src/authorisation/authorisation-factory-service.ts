import { Injectable } from "@angular/core";
import { MsalService } from "@azure/msal-angular";
import { EnvironmentService } from "src/environments/environment.service";
import { AuthorisationBypassService } from "./authorisation-bypass-service";
import { AuthorisationService } from "./authorisation-service";
import { AzureADAuthorisationService } from "./azuread-autorisation-service";
import { IAuthorisationSerivce } from "./iauthorisation-service";

@Injectable({ providedIn: 'root' })
export class AuthorisationFactoryService {
    constructor(private _environment: EnvironmentService, private _msalService: MsalService) {

    }

    private _cache?: IAuthorisationSerivce

    getAuthorisationService(): IAuthorisationSerivce {
        if (this._cache == null) {
            if (this._environment.authEnable == true) {
                if (this._environment.tenantId != undefined && this._environment.tenantId?.length > 1) {
                    this._cache = new AzureADAuthorisationService(this._msalService, this._environment);
                }
                else {
                    this._cache = new AuthorisationService(this._environment);
                }
            }
            else {
                this._cache = new AuthorisationBypassService();
            }
        }
        
        return this._cache;
    }
}