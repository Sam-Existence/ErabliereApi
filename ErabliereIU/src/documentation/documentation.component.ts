import { Component, OnInit } from '@angular/core'
import { EnvironmentService } from 'src/environments/environment.service';

@Component({
    selector: 'documentation',
    templateUrl: "./documentation.component.html"
})
export class DocumentationComponent implements OnInit {
    urlApi?: string
    
    constructor(private _enviromentService: EnvironmentService){}

    ngOnInit(): void {
        this.urlApi = this._enviromentService.apiUrl;
    }
}