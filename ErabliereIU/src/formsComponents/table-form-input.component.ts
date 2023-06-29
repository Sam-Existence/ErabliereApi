import { Component, Input } from "@angular/core";

@Component({
    selector: "table-form-input",
    templateUrl: "./table-form-input.component.html",
})
export class TableFormInputComponent {
    @Input() value?: string | number | boolean;
    @Input() displayEdit?: boolean;

    constructor() { }
}