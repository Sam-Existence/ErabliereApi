import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CustomerAccess} from "../../../model/customerAccess";
import {FormBuilder, FormControl, ReactiveFormsModule, UntypedFormGroup, Validators} from "@angular/forms";
import {Erabliere} from "../../../model/erabliere";
import {AdminSelectErabliereComponent} from "../../../erablieres/admin-select-erabliere.component";

@Component({
  selector: 'tr[add-customer-access-row]',
  standalone: true,
    imports: [
        AdminSelectErabliereComponent,
        ReactiveFormsModule
    ],
  templateUrl: './add-customer-access-row.component.html'
})
export class AddCustomerAccessRowComponent implements OnInit {
    @Input() idCustomer?: string;

    @Output() accesAAnnuler = new EventEmitter<CustomerAccess>();
    @Output() accesAAjouter = new EventEmitter<CustomerAccess>();

    formGroup: UntypedFormGroup;

    acces: CustomerAccess = new CustomerAccess();

    constructor(private formBuilder: FormBuilder) {
        this.formGroup = this.formBuilder.group({
            erabliere: new FormControl(null,
                {
                    validators: [Validators.required],
                    updateOn: 'blur'
                }),
            lecture: new FormControl<boolean>(false),
            creation: new FormControl<boolean>(false),
            modification: new FormControl<boolean>(false),
            suppression: new FormControl<boolean>(false)
        });
    }

    ngOnInit() {
        this.acces.idCustomer = this.idCustomer;
    }

    erabliereSelected(erabliere?: Erabliere) {
        this.acces.idErabliere = erabliere?.id;
    }

    signalerAjout() {
        this.formGroup.controls.erabliere.markAsTouched();
        if (this.formGroup.valid) {
            this.acces.access =
                (this.formGroup?.controls.lecture.value ? 1 : 0)
                +   (this.formGroup.controls.creation.value ? 2 : 0)
                +   (this.formGroup.controls.modification.value ? 4 : 0)
                +   (this.formGroup.controls.suppression.value ? 8 : 0);

            this.accesAAjouter.emit(this.acces);
        }
    }

    signalerAnnulation() {
        this.accesAAnnuler.emit(this.acces)
    }
}
