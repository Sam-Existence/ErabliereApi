import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CustomerAccess } from "../../../model/customerAccess";
import { FormBuilder, FormControl, ReactiveFormsModule, UntypedFormGroup } from "@angular/forms";

@Component({
  selector: 'tr[edit-erabliere-access-row]',
  standalone: true,
    imports: [
        ReactiveFormsModule
    ],
  templateUrl: './edit-erabliere-access-row.component.html'
})
export class EditErabliereAccessRowComponent implements OnInit {
    @Input() acces?: CustomerAccess;

    @Output() accesAAnnuler = new EventEmitter<CustomerAccess>();
    @Output() accesAEnregister = new EventEmitter<CustomerAccess>();

    formGroup: UntypedFormGroup;

    constructor(private formBuilder: FormBuilder) {
        this.formGroup = this.formBuilder.group({
            lecture: new FormControl<boolean>(false),
            creation: new FormControl<boolean>(false),
            modification: new FormControl<boolean>(false),
            suppression: new FormControl<boolean>(false)
        });
    }

    ngOnInit() {
        if (this.acces?.access) {
            this.formGroup.controls.lecture.setValue(!!(this.acces.access & 1));
            this.formGroup.controls.creation.setValue(!!(this.acces.access & 2));
            this.formGroup.controls.modification.setValue(!!(this.acces.access & 4));
            this.formGroup.controls.suppression.setValue(!!(this.acces.access & 8));
        }
    }

    signalerEnregistrement() {
        if (this.acces && this.formGroup?.valid) {
            this.acces.access =
                (this.formGroup?.controls.lecture.value ? 1 : 0)
            +   (this.formGroup.controls.creation.value ? 2 : 0)
            +   (this.formGroup.controls.modification.value ? 4 : 0)
            +   (this.formGroup.controls.suppression.value ? 8 : 0);

            this.accesAEnregister.emit(this.acces);
        }
    }

    signalerAnnulation() {
        this.accesAAnnuler.emit(this.acces);
    }
}
