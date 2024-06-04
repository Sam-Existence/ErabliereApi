import { Directive, ElementRef, forwardRef, HostListener, Renderer2 } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
    selector: '[onlyDigits]',
    standalone: true,
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => OnlyDigitsDirective),
        multi: true
    }]
})
export class OnlyDigitsDirective implements ControlValueAccessor {
    private onChange: (val: string) => void;
    private onTouched: () => void;
    private value: string;

    constructor(
        private elementRef: ElementRef,
        private renderer: Renderer2
    ) {
        this.onChange = () => {};
        this.onTouched = () => {};
        this.value = "";
    }

    @HostListener('input', ['$event.target.value'])
    onInputChange(value: string) {
        const filteredValue: string = filterValue(value);
        this.updateTextInput(filteredValue, this.value !== filteredValue);
    }

    @HostListener('blur')
    onBlur() {
        this.onTouched();
    }

    private updateTextInput(value: string, propagateChange: boolean) {
        this.renderer.setProperty(this.elementRef.nativeElement, 'value', value);
        if (propagateChange) {
            this.onChange(value);
        }
        this.value = value;
    }

    // ControlValueAccessor Interface
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.renderer.setProperty(this.elementRef.nativeElement, 'disabled', isDisabled);
    }

    writeValue(value: any): void {
        value = value ? String(value) : '';
        this.updateTextInput(value, false);
    }
}

function filterValue(value: any): string {
    return value.replace(/\D*/g, '');
}
