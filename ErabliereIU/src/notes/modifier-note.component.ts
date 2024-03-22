import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { UntypedFormGroup, UntypedFormBuilder, FormControl, Validators, ReactiveFormsModule } from "@angular/forms";
import { Note } from "src/model/note";
import { InputErrorComponent } from "../formsComponents/input-error.component";
import { NgIf } from "@angular/common";
import { Subject } from "rxjs";

@Component({
    selector: 'modifier-note',
    templateUrl: 'modifier-note.component.html',
    standalone: true,
    imports: [NgIf, ReactiveFormsModule, InputErrorComponent]
})
export class ModifierNoteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.noteForm = this.fb.group({});
    }

    ngOnInit(): void {
        this.noteSubject?.subscribe(note => {
            this.initializeForm();
            if (note != null) {
                this.note = { ... note };
                if (this.note != null) {
                    this.noteForm.controls['title'].setValue(this.note.title);
                    this.noteForm.controls['text'].setValue(this.note.text);
                    this.noteForm.controls['noteDate'].setValue(this.note.noteDate);
                }
            }
        });
    }

    initializeForm() {
        this.noteForm = this.fb.group({
            title: ['', Validators.required],
            text: new FormControl(''),
            file: new FormControl(''),
            fileBase64: new FormControl(''),
            noteDate: new FormControl(''),
        });
    }

    error: string | null = null;

    @Input() noteSubject?: Subject<Note | undefined>;

    note?: Note;

    @Input() idErabliereSelectionee: any

    @Output() needToUpdate = new EventEmitter();

    noteForm: UntypedFormGroup;

    errorObj: any;

    fileToLargeErrorMessage?: string;

    generalError?: string;

    onSubmit() {

    }

    onButtonAnnuleClick() {
        this.note = undefined;
    }

    onButtonModifierClick() {
        if (this.note != undefined) {
            this.note.title = this.noteForm.controls['title'].value;
            this.note.text = this.noteForm.controls['text'].value;
            if (this.noteForm.controls['noteDate'].value != "") {
                this.note.noteDate = this.noteForm.controls['noteDate'].value;
            }
            else {
                this.note.noteDate = undefined;
            }
            this._api.putNote(this.idErabliereSelectionee, this.note)
                .then(r => {
                    this.errorObj = undefined;
                    this.fileToLargeErrorMessage = undefined;
                    this.generalError = undefined;
                    this.noteForm.reset();
                    this.needToUpdate.emit();
                    this.noteSubject?.next(undefined);
                    this.note = undefined;
                })
                .catch(e => {
                    if (e.status == 400) {
                        this.errorObj = e
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = undefined;
                    }
                    else if (e.status == 404) {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = $localize `:erreurErabliereInexistante:L'érablière n'existe pas.`;
                    }
                    else if (e.status == 405) {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = $localize `:erreurAPIModificationNote:L'API ne permet pas de modifier une note.`;
                    }
                    else if (e.status == 413) {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = $localize `:erreurFichierTropGros:Le fichier est trop gros.`;
                        this.generalError = undefined;
                    }
                    else {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = $localize `:erreurGenerique:Une erreur est survenue.`;
                    }
                });
        }
        else {
            console.log("this.note is undefined");
        }
    }

    convertToBase64(event: any) {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            this.noteForm.controls['fileBase64'].setValue(reader.result?.toString().split(',')[1]);
        };
    }
}