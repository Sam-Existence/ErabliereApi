import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Note } from 'src/model/note';
import { NoteComponent } from './note.component';
import { NgIf, NgFor } from '@angular/common';
import { AjouterNoteComponent } from './ajouter-note.component';
import { ModifierNoteComponent } from './modifier-note.component';
import { Subject } from 'rxjs';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import { ActivatedRoute } from '@angular/router';
import {PaginationComponent} from "../pagination/pagination.component";
import { ca } from 'date-fns/locale';

@Component({
    selector: 'notes',
    templateUrl: "./notes.component.html",
    standalone: true,
    imports: [
        AjouterNoteComponent,
        ModifierNoteComponent,
        NgIf,
        NgFor,
        NoteComponent,
        PaginationComponent
    ]
})
export class NotesComponent implements OnInit {
    @Input() idErabliereSelectionee: any
    @Input() notes?: Note[];
    private _nombreParPage: number = 5;
    private _nombreTotal: number = 0;
    get nombreTotal() {
        return this._nombreTotal;
    }
    get nombreParPage() {
        return this._nombreParPage;
    }
    set nombreParPage(value: number) {
        if(value != this._nombreParPage) {
            this._nombreParPage = value;
            this.loadNotes();
        }
    }
    private _pageActuelle: number = 1;
    set pageActuelle(value: number) {
        if(value != this._pageActuelle) {
            this._pageActuelle = value;
            this.loadNotes();
        }
    }
    @Output() needToUpdate = new EventEmitter();
    noteToModify?: Subject<Note | null> = new Subject<Note | null>();
    error?: string | null;

    constructor(private _api: ErabliereApi, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this._route.paramMap.subscribe(params => {
            this.idErabliereSelectionee = params.get('idErabliereSelectionee');

            if (this.idErabliereSelectionee) {
                this.loadNotes();
            }
        });
    }

    loadNotes() {
        this._api.getNotesCount(this.idErabliereSelectionee).then(count => this._nombreTotal = count);
        this._api.getNotes(this.idErabliereSelectionee, (this._pageActuelle - 1) * this._nombreParPage, this._nombreParPage)
            .then(async notes => {
                this.notes = notes;
                this.error = null;

                for (let i = 0; i < notes.length; i++) {
                    let n = notes[i];
                    if (n.fileExtension == 'csv') {
                        n.decodedTextFile = atob(n.file ?? "");
                    }
                    else {
                        try {
                            let data = await this._api.getNoteImage(n.idErabliere, n.id);

                            if (data) {
                                let binary = '';
                                let bytes = new Uint8Array(data);
                                let len = bytes.byteLength;
                                for (let i = 0; i < len; i++) {
                                    binary += String.fromCharCode(bytes[i]);
                                }
                                n.file = btoa(binary);
                            }
                        }
                        catch (e) {
                            console.error(e);
                        }
                    }
                }
            })
            .catch(errorBody => {
                this.notes = [];
                this.error = "";

                console.log(errorBody)

                if (errorBody.status == 404) {
                    this.error = "Érablière '" + this.idErabliereSelectionee + "' introuvable";
                }
                else {
                    if (errorBody.errors) {
                        for (let key in errorBody.errors) {
                            this.error += errorBody[key];
                        }
                    }
                    else {
                        this.error = errorBody;
                    }
                }
            });
    }
}
