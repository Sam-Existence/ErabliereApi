import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from "@angular/core";
import { NgIf } from "@angular/common";

@Component({
    selector: "table-form-input",
    template: `
        <span *ngIf="!displayEdit">
            {{value}}
        </span>
        <span *ngIf="displayEdit">
            <input 
                [value]="value"
                [checked]="value"
                class="{{ class }}"
                type="{{ type }}" />
        </span>
    `,
    standalone: true,
    imports: [NgIf],
})
export class TableFormInputComponent implements OnInit {
    @Input() value?: string | number | boolean;
    @Input() displayEdit?: boolean;
    @Input() type: "text" | "number" | "checkbox" = "text";
    class: string = "form-control form-control-sm";

    constructor() { }

    ngOnInit(): void {
        if (this.type === "checkbox") {
            this.class = "form-check-input";
        }
    }
}