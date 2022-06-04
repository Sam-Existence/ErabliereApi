import { Component, EventEmitter, Input, OnInit, Output, SimpleChange } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'date-time-selector',
    template: `
        <span>{{titre}}</span>
        <input class="form-control" type="date" [formControl]="dateControl"/>
    `
})
export class DateTimeSelector implements OnInit {
    @Input() titre?: string;
    currentVal: any;
    previousVal: any;
    @Output() onChange: EventEmitter<SimpleChange> = new EventEmitter();

    dateControl = new FormControl('');

    constructor() { }

    ngOnInit() { 
        this.dateControl.valueChanges.subscribe(value => {
            this.previousVal = this.currentVal;
            this.currentVal = value;
            this.onChange.emit(new SimpleChange(this.previousVal, this.currentVal, false));
        });
    }
}