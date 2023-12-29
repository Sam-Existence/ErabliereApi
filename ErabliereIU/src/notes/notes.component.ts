import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/model/note';
import { NoteComponent } from './note.component';
import { NgIf, NgFor } from '@angular/common';
import { AjouterNoteComponent } from './ajouter-note.component';
import { ModifierNoteComponent } from './modifier-note.component';
import { Subject } from 'rxjs';

@Component({
    selector: 'notes',
    templateUrl: "./notes.component.html",
    standalone: true,
    imports: [AjouterNoteComponent, ModifierNoteComponent, NgIf, NgFor, NoteComponent]
})
export class NotesComponent implements OnInit {
    @Input() idErabliereSelectionee:any

    @Input() notes?: Note[];

    @Output() needToUpdate = new EventEmitter();
    
    noteToModify?: Subject<Note | undefined> = new Subject<Note | undefined>();

    constructor () { }

    ngOnInit(): void { }

    updateNotes(event:any) {
        this.needToUpdate.emit();
    }
}