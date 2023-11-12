import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/model/note';
import { NoteComponent } from './note.component';
import { NgIf, NgFor } from '@angular/common';
import { AjouterNoteComponent } from './ajouter-note.component';

@Component({
    selector: 'notes',
    templateUrl: "./notes.component.html",
    standalone: true,
    imports: [AjouterNoteComponent, NgIf, NgFor, NoteComponent]
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