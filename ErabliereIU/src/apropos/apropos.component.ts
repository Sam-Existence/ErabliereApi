import { Component } from "@angular/core";
import { EnvironmentService } from "src/environments/environment.service";
import { ErabliereApi } from "src/core/erabliereapi.service";

@Component({
    selector: 'apropos',
    templateUrl: "./apropos.component.html"
})
export class AProposComponent {
    urlApi?: string
    checkoutEnabled?: boolean
    
    constructor(private _enviromentService: EnvironmentService, private _erbliereApi: ErabliereApi){}

    ngOnInit(): void {
        this.urlApi = this._enviromentService.apiUrl;
    }

    buyApiKey(): void {
        this._erbliereApi.startCheckoutSession().then(resonse => {
            window.location.href = resonse.url;
        });
    }
}