import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/model/note';

@Component({
    selector: 'notes',
    templateUrl: "./notes.component.html"
})
export class NotesComponent implements OnInit {
    @Input() idErabliereSelectionee:any

    @Input() notes?: Note[];

    @Output() needToUpdate = new EventEmitter();
    
    constructor () { }

    ngOnInit(): void { }

    updateNotes(event:any) {
        this.needToUpdate.emit();
    }
}