import { Component, Input } from "@angular/core";
import { NgIf } from "@angular/common";

@Component({
    selector: "table-form-input",
    templateUrl: "./table-form-input.component.html",
    standalone: true,
    imports: [NgIf],
})
export class TableFormInputComponent {
    @Input() value?: string | number | boolean;
    @Input() displayEdit?: boolean;

    constructor() { }
}