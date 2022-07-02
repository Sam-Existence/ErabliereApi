import { Component, Input, OnInit } from '@angular/core';
import { CustomerAccess } from 'src/model/customerAccess';

@Component({
    selector: 'edit-access',
    templateUrl: 'edit-access.component.html'
})

export class EditAccessCompoenent implements OnInit {
    @Input() displayEditAccess: Boolean = false;
    @Input() acces?: CustomerAccess;

    constructor() { }

    ngOnInit() { }
}