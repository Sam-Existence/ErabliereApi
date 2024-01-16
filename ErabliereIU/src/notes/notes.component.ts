import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/model/note';
import { NoteComponent } from './note.component';
import { NgIf, NgFor } from '@angular/common';
import { AjouterNoteComponent } from './ajouter-note.component';
import { ModifierNoteComponent } from './modifier-note.component';
import { Subject } from 'rxjs';
import { ErabliereApi } from 'src/core/erabliereapi.service';

@Component({
    selector: 'notes',
    templateUrl: "./notes.component.html",
    standalone: true,
    imports: [AjouterNoteComponent, ModifierNoteComponent, NgIf, NgFor, NoteComponent]
})
export class NotesComponent implements OnInit {
    @Input() idErabliereSelectionee: any

    @Input() notes?: Note[];

    @Output() needToUpdate = new EventEmitter();

    noteToModify?: Subject<Note | undefined> = new Subject<Note | undefined>();

    error?: string;

    constructor(private _api: ErabliereApi) { }

    ngOnInit(): void {
        this.loadNotes();
    }

    loadNotes() {
        this._api.getNotes(this.idErabliereSelectionee)
            .then(notes => {
                notes.forEach(n => {
                    if (n.fileExtension == 'csv') {
                        n.decodedTextFile = atob(n.file ?? "");
                    }
                });

                this.notes = notes;
                this.error = undefined;
            })
            .catch(errorBody => {
                this.notes = undefined;
                this.error = "";
                for (let key in errorBody.errors) {
                    this.error += errorBody[key];
                }
            });
    }

    updateNotes(event: any) {
        this.needToUpdate.emit();
    }
}