import { Component, Input, OnInit } from '@angular/core'
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Note } from 'src/model/note';

@Component({
    selector: 'note',
    templateUrl: "./note.component.html"
})
export class NoteComponent implements OnInit {
    @Input() idErabliereSelectionee:any

    @Input() notes?: Note[];
    
    constructor (private _api: ErabliereApi) {
        
    }

    ngOnInit(): void {
        
    }
}