import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { UntypedFormGroup, UntypedFormBuilder, FormControl, Validators, ReactiveFormsModule } from "@angular/forms";
import { Note } from "src/model/note";
import { InputErrorComponent } from "../formsComponents/input-error.component";
import { NgIf } from "@angular/common";

@Component({
    selector: 'ajouter-note',
    templateUrl: 'ajouter-note.component.html',
    standalone: true,
    imports: [NgIf, ReactiveFormsModule, InputErrorComponent]
})
export class AjouterNoteComponent implements OnInit {
    set displayReminder(value: boolean) {
        this._displayReminder = value;
    }
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.noteForm = this.fb.group({});
    }

    ngOnInit(): void {
        this.initializeForm();
        this.noteForm.controls['reminderEnabled'].valueChanges.subscribe((value) => {
            this._displayReminder = value;
        });
    }

    initializeForm() {
        this.noteForm = this.fb.group({
            title: ['', Validators.required],
            text: new FormControl(''),
            file: new FormControl(''),
            fileBase64: new FormControl(''),
            noteDate: new FormControl(''),
            reminderEnabled: new FormControl(false),
            reminderDate: new FormControl(''),
        });
    }

    display:boolean = false;

    private _displayReminder:boolean = false;

    date = new Date();
    year = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(this.date);
    month = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(this.date);
    day = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(this.date);
    today = `${this.year}-${this.month}-${this.day}`;

    error: string | null = null;

    note:Note = new Note();

    @Input() notes?: Note[];

    @Input() idErabliereSelectionee:any

    @Output() needToUpdate = new EventEmitter();

    noteForm: UntypedFormGroup;

    errorObj: any;

    fileToLargeErrorMessage?: string;

    generalError?: string;

    onSubmit() {

    }

    // toggleReminder() {
    //     this.displayReminder = this.noteForm.controls['reminderEnabled'].value;
    // }
    get displayReminder(): boolean {
        return this.noteForm.controls['reminderEnabled'].value;
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
            this.note.file = this.noteForm.controls['fileBase64'].value;
            if (this.noteForm.controls['noteDate'].value != "") {
                this.note.noteDate = this.noteForm.controls['noteDate'].value;
            }
            else {
                this.note.noteDate = undefined;
            }
            if (this.noteForm.controls['reminderEnabled'].value && this.noteForm.controls['reminderDate'].value) {
                let date = new Date(this.noteForm.controls['reminderDate'].value);
                this.note.reminderDate = date.toISOString();
            }
            else {
                this.note.reminderDate = undefined;
            }
            this._api.postNote(this.idErabliereSelectionee, this.note)
                     .then(r => {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = undefined;
                        this.noteForm.reset();
                        this.needToUpdate.emit();
                      })
                      .catch(e => {
                        if (e.status == 400) {
                            this.errorObj = e
                            this.fileToLargeErrorMessage = undefined;
                            this.generalError = this.errorObj.error.errors['postNote'];
                        }
                        else if (e.status == 413) {
                            this.errorObj = undefined;
                            this.fileToLargeErrorMessage = "Le fichier est trop gros."
                            this.generalError = undefined;
                        }
                        else {
                            this.errorObj = undefined;
                            this.fileToLargeErrorMessage = undefined;
                            this.generalError = "Une erreur est survenue. " + this.errorObj.error.errors['postNote'];
                        }
                      });
        }
        else {
            console.log("this.note is undefined");
        }
    }

    convertToBase64(event:any) {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            this.noteForm.controls['fileBase64'].setValue(reader.result?.toString().split(',')[1]);
        };
    }
}
