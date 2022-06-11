import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Note } from 'src/model/note';

@Component({
    selector: 'note',
    template: `
        <div id="note-{{ note.id }}">
            <div class="row">
                <div class="col-md-4">
                    <h4 class="mt-4">{{ note.title }}</h4>
                </div>

                <div class="col-md-8">
                    <button class="btn btn-danger btn-sm" (click)="deleteNote()">Supprimer</button>
                </div>
            </div>

            <div class="row">
                <p class="noteDate">{{ note.noteDate }}</p>
            </div>

            <div class="row">
                <p class="noteDescription">{{ note.text }}</p>
            </div>

            <div *ngIf="note.fileExtension == 'csv'">
                <p>{{ note.decodedTextFile }}</p>
            </div>

            <div *ngIf="note.fileExtension != 'csv'" class="row">
                <img *ngIf="note.file != ''" src="data:image/png;base64,{{ note.file }}" />
            </div>
        </div>
    `
})

export class NoteComponent implements OnInit {
    constructor(private _api: ErabliereApi) { 
        this.note = new Note();
    }

    ngOnInit() { }

    @Input() note: Note;

    deleteNote() {
        this._api.deleteNote(this.note.idErabliere, this.note.id).then(
            (data) => {
                this.needToUpdate.emit();
            }
        );
    }

    @Output() needToUpdate = new EventEmitter();
}