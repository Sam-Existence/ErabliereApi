import { Component, Input, OnInit } from '@angular/core';
import { Note } from 'src/model/note';

@Component({
    selector: 'note',
    template: `
        <div id="note-{{ note.id }}">
            <div class="row">
                <h4 class="mt-4">{{ note.title }}</h4>
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
    constructor() { 
        this.note = new Note();
    }

    ngOnInit() { }

    @Input() note: Note;
}