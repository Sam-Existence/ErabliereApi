import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { UntypedFormGroup, UntypedFormBuilder, FormControl, Validators } from "@angular/forms";
import { Note } from "src/model/note";

@Component({
    selector: 'ajouter-note',
    templateUrl: 'ajouter-note.component.html'
})
export class AjouterNoteComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.noteForm = this.fb.group({});
    }
    
    ngOnInit(): void {
        this.initializeForm();
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
    
    display:boolean = false;

    error: string | null = null;

    note:Note = new Note();

    @Input() notes?: Note[];

    @Input() idErabliereSelectionee:any

    @Output() needToUpdate = new EventEmitter();

    noteForm: UntypedFormGroup;

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
            this.note.file = this.noteForm.controls['fileBase64'].value;
            if (this.noteForm.controls['noteDate'].value != "") {
                this.note.noteDate = this.noteForm.controls['noteDate'].value;
            }
            else {
                this.note.noteDate = undefined;
            }
            this._api.postNote(this.idErabliereSelectionee, this.note)
                     .then(r => {
                        this.noteForm.reset();
                        this.needToUpdate.emit();
                      })
                      .catch(e => {
                        if (e.status == 400) {
                            if (e.error.errors.Title != undefined) {
                                this.noteForm.controls['title'].setErrors({
                                    'incorrect': true,
                                    'message': e.error.errors.Title.join(', ')
                                })
                            }
                            if (e.error.errors.Text != undefined) {
                                this.noteForm.controls['text'].setErrors({
                                    'incorrect': true,
                                    'message': e.error.errors.Text.join(', ')
                                })
                            }
                            if (e.error.errors.FileBase64 != undefined) {
                                this.noteForm.controls['file'].setErrors({
                                    'incorrect': true,
                                    'message': e.error.errors.FileBase64.join(', ')
                                })
                            }
                            if (e.error.errors['$.noteDate'] != undefined) {
                                this.noteForm.controls['noteDate'].setErrors({
                                    'incorrect': true,
                                    'message': e.error.errors['$.noteDate'].join(', ')
                                })
                            }
                        }
                        else {
                            throw e;
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