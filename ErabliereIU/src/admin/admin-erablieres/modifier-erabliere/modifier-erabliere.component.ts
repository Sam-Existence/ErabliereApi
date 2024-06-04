import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {
    FormControl,
    FormsModule,
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators
} from "@angular/forms";
import {ErabliereApi} from "../../../core/erabliereapi.service";
import {Erabliere} from "../../../model/erabliere";
import {InputErrorComponent} from "../../../formsComponents/input-error.component";

@Component({
  selector: 'modifier-erabliere-modal',
  standalone: true,
    imports: [
        FormsModule,
        InputErrorComponent,
        ReactiveFormsModule
    ],
  templateUrl: './modifier-erabliere.component.html'
})
export class ModifierErabliereComponent implements OnInit {
    @Input() erabliere: Erabliere | null = null;
    @Output() needToUpdate: EventEmitter<boolean> = new EventEmitter();

    erabliereForm: UntypedFormGroup;
    errorObj?: any;
    generalError?: string | null;

    constructor(private _api: ErabliereApi, private fb: UntypedFormBuilder) {
        this.erabliereForm = this.fb.group({});
    }

    ngOnInit() {
        this.initializeForm();
    }

    initializeForm() {
        this.erabliereForm = this.fb.group({
            nom: new FormControl(
                this.erabliere?.nom,
                {
                    validators: [Validators.required],
                    updateOn: 'blur'
                }
            ),
            codePostal: new FormControl(
                this.erabliere?.codePostal,
                {
                    updateOn: 'blur'
                }
            ),
            isPublic: new FormControl(
                this.erabliere?.isPublic,
                {
                    validators: [Validators.required],
                    updateOn: 'blur'
                }
            )
        });
    }

    validateForm() {
        const form = document.getElementById("modifier-erabliere");
        this.erabliereForm.updateValueAndValidity();
        form?.classList.add('was-validated');
    }

    onModifier() {
        if (!this.erabliere) {
            return;
        }

        this.validateForm();
        if (!this.erabliereForm.valid) {
            return;
        }

        this.erabliere.nom = this.erabliereForm.controls['nom'].value;
        this.erabliere.codePostal = this.erabliereForm.controls['codePostal'].value;
        this.erabliere.isPublic = this.erabliereForm.controls['isPublic'].value;

        this._api.putErabliereAdmin(this.erabliere)
            .then(r => {
                this.errorObj = null;
                this.generalError = null;
                this.erabliereForm.reset();
                this.needToUpdate.emit(true);
            }).catch(e => {
            if (e.status == 400) {
                this.errorObj = e;
                this.generalError = "Une erreur de validation s'est produite";
            } else if (e.status == 404) {
                this.errorObj = null;
                this.generalError = "L'érablière n'existe pas."
            } else {
                this.errorObj = null;
                this.generalError = "Une erreur est survenue."
            }
        });
    }

    onAnnuler() {
        this.needToUpdate.emit(false);
    }

}
