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
                type="{{ type }}" 
                (change)="onChange($event)"/>
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
    @Output() valueChange = new EventEmitter<string | number | boolean>();

    constructor() { }

    ngOnInit(): void {
        if (this.type === "checkbox") {
            this.class = "form-check-input";
        }
    }

    onChange(event: Event) {
        if (this.type === "checkbox") {
            const target = event.target as HTMLInputElement;
            this.valueChange.emit(target.checked);
            return;
        }
        else {
            const target = event.target as HTMLInputElement;
            this.valueChange.emit(target.value);
        }
    }
}