import { Component, Input } from "@angular/core";
import { NgIf, NgFor } from "@angular/common";

@Component({
    selector: 'input-error',
    template: `
        <div *ngIf="errorObj?.error?.errors != null && errorObj.error.errors.hasOwnProperty(this.controlName)">
            <span class="text-danger" *ngFor="let error of errorObj.error.errors[this.controlName]">{{error}}</span>
        </div>
        <div *ngIf="errorObj?.error?.errors != null && errorObj.error.errors.hasOwnProperty('$.' + this.controlName)">
            <span class="text-danger" *ngFor="let error of errorObj.error.errors['$.' + this.controlName]">{{error}}</span>
        </div>
    `,
    standalone: true,
    imports: [NgIf, NgFor]
})
export class InputErrorComponent {
    @Input() errorObj?: any
    @Input() controlName: string = ""
}