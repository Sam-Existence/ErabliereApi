import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormControl, UntypedFormBuilder, UntypedFormGroup, Validators, ReactiveFormsModule } from "@angular/forms";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { ErabliereApiDocument } from "src/model/erabliereApiDocument";
import { InputErrorComponent } from "../formsComponents/input-error.component";
import { NgIf } from "@angular/common";

@Component({
    selector: 'app-ajouter-documentation',
    templateUrl: 'ajouter-documentation.component.html',
    standalone: true,
    imports: [
        NgIf,
        ReactiveFormsModule,
        InputErrorComponent,
    ],
})
export class AjouterDocumentationComponent implements OnInit {
    display:boolean = false;
    generalError?:string = undefined;
    documentForm: UntypedFormGroup;
    errorObj: any;
    fileToLargeErrorMessage?:string = undefined;
    document: ErabliereApiDocument = new ErabliereApiDocument();
    @Input() idErabliereSelectionee:any
    @Output() needToUpdate = new EventEmitter();

    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.documentForm = this.fb.group({});
    }

    ngOnInit(): void {
        this.initializeForm();
    }

    initializeForm() {
        this.documentForm = this.fb.group({
            title: ['', Validators.required],
            text: new FormControl(''),
            file: new FormControl(''),
            fileExtension: new FormControl(''),
            fileBase64: new FormControl(''),
        });
    }

    onButtonAjouterClick() {
        this.display = true;
    }

    onButtonAnnulerClick() {
        this.display = false;
    }

    convertToBase64(event:any) {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            this.documentForm.controls['fileBase64'].setValue(reader.result?.toString().split(',')[1]);
            this.documentForm.controls['fileExtension'].setValue(file.name.split('.').pop());
        };
    }

    onButtonCreerClick() {
        console.log(this.document);
        if (this.document != undefined) {
            this.document.idErabliere = this.idErabliereSelectionee;
            this.document.title = this.documentForm.controls['title'].value;
            this.document.text = this.documentForm.controls['text'].value;
            this.document.file = this.documentForm.controls['fileBase64'].value;
            this.document.fileExtension = this.documentForm.controls['fileExtension'].value;
            this._api.postDocument(this.idErabliereSelectionee, this.document)
                     .then(r => {
                        this.errorObj = undefined;
                        this.fileToLargeErrorMessage = undefined;
                        this.generalError = undefined;
                        this.documentForm.reset();
                        this.needToUpdate.emit();
                      })
                      .catch(e => {
                        if (e.status == 400) {
                            this.errorObj = e
                            this.fileToLargeErrorMessage = undefined;
                            this.generalError = undefined;
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
}