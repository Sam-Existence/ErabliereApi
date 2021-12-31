import { Component } from "@angular/core";
import { EnvironmentService } from "src/environments/environment.service";

@Component({
    selector: 'apropos',
    templateUrl: "./apropos.component.html"
})
export class AProposComponent {
    urlApi?: string
    
    constructor(private _enviromentService: EnvironmentService){}

    ngOnInit(): void {
        this.urlApi = this._enviromentService.apiUrl;
    }
}