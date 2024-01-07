import { NgIf } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from "@angular/forms";
import { Subject } from "rxjs";
import { ErabliereApi } from "src/core/erabliereapi.service";
import { InputErrorComponent } from "src/formsComponents/input-error.component";
import { ErabliereApiDocument } from "src/model/erabliereApiDocument";

@Component({
    selector: 'modifier-documentation',
    templateUrl: 'modifier-documentation.component.html',
    standalone: true,
    imports: [
        NgIf,
        ReactiveFormsModule,
        InputErrorComponent
    ]
})
export class ModifierDocumentationComponent implements OnInit {
    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.documentForm = this.fb.group({});
    }

    @Input() documentationSubject?: Subject<ErabliereApiDocument | undefined>;
    @Input() idErabliereSelectionee?: any;
    @Output() needToUpdate = new EventEmitter();
    documentForm: UntypedFormGroup;
    display: boolean = false;
    generalError?: string;
    errorObj: any;

    ngOnInit() {
        this.documentationSubject?.subscribe(async (doc) => {
            if (doc != undefined) {
                this.display = true;
                this.documentForm = this.fb.group({
                    id: doc.id,
                    idErabliere: doc.idErabliere,
                    title: doc.title,
                    text: doc.text
                });
            }
            else {
                this.display = false;
            }
        });
    
    }

    onButtonAnnulerClick() {
        this.documentationSubject?.next(undefined);
    }
        
    onButtonModifierClick() {
        const putDocument = {
            id: this.documentForm.controls['id'].value,
            idErabliere: this.documentForm.controls['idErabliere'].value,
            title: this.documentForm.controls['title'].value,
            text: this.documentForm.controls['text'].value
        };
        this._api.putDocumentation(this.idErabliereSelectionee, putDocument).then(() => {
            this.documentationSubject?.next(undefined);
            this.needToUpdate.emit();
        }).catch((err) => {
            this.generalError = err.message;
            this.errorObj = err;
        });
    }
}