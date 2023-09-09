import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { EnvironmentService } from 'src/environments/environment.service';
import { Documentation } from 'src/model/documentation';

@Component({
    selector: 'documentation',
    templateUrl: "./documentation.component.html"
})
export class DocumentationComponent implements OnInit {
    @Input() idErabliereSelectionee:any

    @Input() documentations?: Documentation[];

    @Output() needToUpdate = new EventEmitter();
    
    constructor (private _api: ErabliereApi) {
        
    }

    ngOnInit(): void {
        
    }

    updateDocuments() {
        this.needToUpdate.emit();
    }
}