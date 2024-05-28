import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { Note } from 'src/model/note';
import {DatePipe, NgIf} from '@angular/common';
import { Subject } from 'rxjs';

@Component({
    selector: 'note',
    templateUrl: 'note.component.html',
    standalone: true,
    imports: [NgIf, DatePipe]
})

export class NoteComponent {
    @Input() note: Note;
    @Input() noteToModifySubject?: Subject<Note | null>;
    @Output() needToUpdate = new EventEmitter();

    constructor(private _api: ErabliereApi) {
        this.note = new Note();
    }

    selectEditNote() {
        if (this.noteToModifySubject === null) {
            console.error("noteToModifySubject is null");
            return;
        }

        this.noteToModifySubject?.next(this.note);
    }

    deleteNote() {
        this._api.deleteNote(this.note.idErabliere, this.note.id).then(
            (data) => {
                this.needToUpdate.emit();
            }
        );
    }
}
