import { Component, Input, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { NgxMaskDirective } from "ngx-mask";

@Component({
    selector: 'einput',
    template: `
        <div [formGroup]="formGroup" class="input-group">
            <input 
                class="form-control" 
                type="text" 
                formControlName="{{ name }}" 
                name="{{ name }}"
                placeholder="{{ placeholder }}"
                [decimalMarker]="decimalMarker"
                [mask]="textMask"
                [patterns]="customPatterns"
                [specialCharacters]="spChar"
                [dropSpecialCharacters]="false"
                [allowNegativeNumbers]="true">
            <div *ngIf="symbole" class="input-group-append">
                <span class="input-group-text">{{ symbole }}</span>
            </div>
        </div>
    `
})
export class EinputComponent implements OnInit {
    @Input() arialabel?: string
    @Input() symbole?: string
    @Input() name: string = ""
    @Input() formGroup: FormGroup<any> | any;
    @Input() placeholder = "0.0"
    @Input() textMask?: string = "separator.1"
    @Input() decimalMarker: "." | "," | [".", ","] = ".";
    @Input() customPatterns: any
    @Input() spChar: string[] = []

    constructor() {

    }

    ngOnInit(): void {
        
    }
}