import { Injectable } from "@angular/core";
import { EnvironmentService } from "src/environments/environment.service";
import { AuthorisationBypassService } from "./authorisation-bypass-service";
import { AuthorisationService } from "./authorisation-service";
import { AzureADAuthorisationService } from "./azuread-autorisation-service";
import { IAuthorisationSerivce } from "./iauthorisation-service";

@Injectable({ providedIn: 'root' })
export class AuthorisationFactoryService {
    constructor(private _environment: EnvironmentService) {

    }

    getAuthorisationService(): IAuthorisationSerivce {
        if (this._environment.authEnable == true) {
            if (this._environment.tenantId != undefined && this._environment.tenantId?.length > 1) {
                return new AzureADAuthorisationService(this._environment);
            }
            else {
                return new AuthorisationService(this._environment);
            }
        }
        else {
            return new AuthorisationBypassService();
        }
    }
}