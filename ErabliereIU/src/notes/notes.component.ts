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
    noteToModify?: Subject<Note | undefined> = new Subject<Note | undefined>();
    error?: string;

    constructor(private _api: ErabliereApi, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this._route.paramMap.subscribe(params => {
            this.idErabliereSelectionee = params.get('idErabliereSelectionee');

            if (this.idErabliereSelectionee) {
                this.loadNotes();
            }
        });
        this.loadNotes();
    }

    loadNotes() {
        this._api.getNotes(this.idErabliereSelectionee, (this._pageActuelle - 1) * this._nombreParPage, this._nombreParPage)
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
                this.notes = [];
                this.error = "";

                console.log(errorBody)

                if (errorBody.status == 404) {
                    this.error = $localize `:erabliereIntrouvale:Érablière '${this.idErabliereSelectionee}' introuvable`;
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
