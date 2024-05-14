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
            if (note) {
                this.note = { ... note };
                if (this.note) {
                    this.noteForm.controls['title'].setValue(this.note.title);
                    this.noteForm.controls['text'].setValue(this.note.text);
                    this.noteForm.controls['noteDate'].setValue(this.note.noteDate);
                    this.noteForm.controls['reminderDate'].setValue(this.note.reminderDate ? new Date(this.note.reminderDate).toISOString().split('T')[0] : '');                }
            }
        });
    }

    initializeForm() {
      this.noteForm = this.fb.group({
        title: new FormControl(
          '',
          {
            validators: [Validators.required, Validators.maxLength(200)],
            updateOn: 'blur'
          }),
        text: new FormControl(
          '',
          {
            validators: [Validators.maxLength(2000)],
            updateOn: 'blur'
          }),
        file: new FormControl(
          '',
          {
            updateOn: 'blur'
          }
        ),
        fileBase64: new FormControl(
          '',
          {
            updateOn: 'blur'
          }
        ),
        noteDate: new FormControl(
          '',
          {
            updateOn: 'blur'
          }
        ),
        reminderDate: new FormControl(
          '',
          {
            updateOn: 'blur'
          }
        ),
      });
    }

    error: string | null = null;

    @Input() noteSubject?: Subject<Note | null>;

    note?: Note | null;

    @Input() idErabliereSelectionee: any

    @Output() needToUpdate = new EventEmitter();

    noteForm: UntypedFormGroup;

    errorObj: any;

    fileToLargeErrorMessage?: string | null;

    generalError?: string | null;

    today = new Intl.DateTimeFormat('fr-ca', { year: 'numeric', month: '2-digit', day: '2-digit' }).format(new Date());

    validateForm() {
      const form = document.getElementById('modifier-note');
      this.noteForm.updateValueAndValidity();
      form?.classList.add('was-validated');
    }

    onButtonAnnuleClick() {
        this.note = null;
    }

    onButtonModifierClick() {
        if (this.note) {
          if(this.noteForm.valid) {
            this.note.title = this.noteForm.controls['title'].value;
            this.note.text = this.noteForm.controls['text'].value;
            if (this.noteForm.controls['noteDate'].value != "") {
              this.note.noteDate = this.noteForm.controls['noteDate'].value;
            }
            else {
              this.note.noteDate = null;
            }
            if (this.noteForm.controls['reminderDate'].value !== "") {
              this.note.reminderDate = this.noteForm.controls['reminderDate'].value;
            }
            else {
              this.note.reminderDate = null;
            }
            this._api.putNote(this.idErabliereSelectionee, this.note)
              .then(r => {
                this.errorObj = null;
                this.fileToLargeErrorMessage = null;
                this.generalError = null;
                this.noteForm.reset();
                this.needToUpdate.emit();
                this.noteSubject?.next(null);
                this.note = null;
              })
              .catch(e => {
                if (e.status == 400) {
                  this.errorObj = e
                  this.fileToLargeErrorMessage = null;
                  this.generalError = null;
                }
                else if (e.status == 404) {
                  this.errorObj = null;
                  this.fileToLargeErrorMessage = null;
                  this.generalError = "L'érablière n'existe pas."
                }
                else if (e.status == 405) {
                  this.errorObj = null;
                  this.fileToLargeErrorMessage = null;
                  this.generalError = "L'API ne permet pas de modifier une note."
                }
                else if (e.status == 413) {
                  this.errorObj = null;
                  this.fileToLargeErrorMessage = "Le fichier est trop gros."
                  this.generalError = null;
                }
                else {
                  this.errorObj = null;
                  this.fileToLargeErrorMessage = null;
                  this.generalError = "Une erreur est survenue."
                }
              });
          } else {
            this.validateForm();
          }
        }
        else {
            console.log("this.note is null");
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
