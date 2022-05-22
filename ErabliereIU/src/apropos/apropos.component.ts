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
    supportEmail?: string
    
    constructor(private _enviromentService: EnvironmentService, private _erbliereApi: ErabliereApi){}

    ngOnInit(): void {
        this.urlApi = this._enviromentService.apiUrl;

        this._erbliereApi.getOpenApiSpec().then(spec => {
            console.log(spec);
            this.supportEmail = spec.info.contact.email;
            this.checkoutEnabled = spec.paths['/Checkout'] !== undefined;
        });
    }

    buyApiKey(): void {
        this._erbliereApi.startCheckoutSession().then(resonse => {
            window.location.href = resonse.url;
        });
    }
}