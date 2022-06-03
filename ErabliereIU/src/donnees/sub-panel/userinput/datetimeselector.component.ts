import { Component, EventEmitter, Input, OnInit, Output, SimpleChange } from '@angular/core';

@Component({
    selector: 'date-time-selector',
    template: `
        <span>{{titre}}</span>
        <input class="form-control" type="date" />
    `
})
export class DateTimeSelector implements OnInit {
    @Input() titre?: string;
    currentVal: any;
    previousVal: any;
    @Output() onChange: EventEmitter<SimpleChange> = new EventEmitter();

    constructor() { }

    ngOnInit() { }

    ngOnChanges(changes: SimpleChange) {
        this.currentVal = changes.currentValue;
        this.previousVal = changes.previousValue;
        this.onChange.emit(changes);
    }
}