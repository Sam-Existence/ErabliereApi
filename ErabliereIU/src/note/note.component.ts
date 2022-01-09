import { Component, Input, OnInit } from '@angular/core';
import { Note } from 'src/model/note';

@Component({
    selector: 'note',
    templateUrl: "./note.component.html"
})
export class NoteComponent implements OnInit {
    @Input() idErabliereSelectionee:any

    @Input() notes?: Note[];
    
    constructor () { }

    ngOnInit(): void { }
}