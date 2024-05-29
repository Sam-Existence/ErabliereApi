import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErabliereApi } from 'src/core/erabliereapi.service';
import {FormBuilder, FormControl, ReactiveFormsModule, UntypedFormGroup, Validators} from '@angular/forms';
import {Erabliere} from "../model/erabliere";

@Component({
    selector: 'admin-select-erabliere',
    templateUrl: 'select-erabliere.component.html',
    standalone: true,
    imports: [ReactiveFormsModule]
})

export class AdminSelectErabliereComponent implements OnInit {
    @Input() form: UntypedFormGroup;
    @Output() erabliereSelected = new EventEmitter<Erabliere>();

    erablieres: Erabliere[] = [];

    constructor(private _api: ErabliereApi, private formBuilder: FormBuilder) {
        this.form = this.formBuilder.group({
            erabliere: new FormControl(null,
                {
                    validators: [Validators.required],
                    updateOn: 'blur'
                })
        });
    }

    ngOnInit() {
        this._api.getErablieresAdmin().then(customers => {
            this.erablieres = customers;
        })
        .catch(error => {
            this.erablieres = [];
            throw error;
        });
    }

    onSelectChange(event: any) {
        let erabliere;
        if (event.target.value) {
            erabliere = this.erablieres.find(c => c.id === event.target.value);
        }
        this.erabliereSelected.emit(erabliere);
    }
}
