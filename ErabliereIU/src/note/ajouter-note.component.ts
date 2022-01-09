import { Component, Input, OnInit } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { FormGroup, FormBuilder } from "@angular/forms";
import { Note } from "src/model/note";

@Component({
    selector: 'ajouter-note',
    templateUrl: 'ajouter-note.component.html'
})
export class AjouterNoteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: FormBuilder) {
        this.noteForm = this.fb.group({});
    }
    
    ngOnInit(): void {
        this.initializeForm();
    }

    initializeForm() {
        this.noteForm = this.fb.group({
            title: '',
            text: '',
            file: '',
            noteDate: '',
        });
    }
    
    display:boolean = false;

    note:Note = new Note();

    @Input() notes?: Note[];

    @Input() idErabliereSelectionee:any

    noteForm: FormGroup;

    onSubmit() {

    }

    onButtonAjouterClick() {
        this.display = true;
    }

    onButtonAnnuleClick() {
        this.display = false;
    }

    onButtonCreerClick() {
        if (this.note != undefined) {
            this.note.idErabliere = this.idErabliereSelectionee;
            this.note.title = this.noteForm.controls['title'].value;
            this.note.text = this.noteForm.controls['text'].value;
            this.note.file = this.noteForm.controls['file'].value;
            if (this.noteForm.controls['noteDate'].value != "") {
                this.note.noteDate = this.noteForm.controls['noteDate'].value;
            }
            this._api.postNote(this.idErabliereSelectionee, this.note)
                .then(r => {
                    this.display = false;
                });
        }
        else {
            console.log("this.note is undefined");
        }
    }
}