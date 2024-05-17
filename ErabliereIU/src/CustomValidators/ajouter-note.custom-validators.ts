
import {AbstractControl, ValidationErrors, ValidatorFn} from "@angular/forms";

// Validator for dateRappel
export function dateRappelValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const selectedDate = new Date(control.value);
        selectedDate.setHours(0, 0, 0, 0);
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        return selectedDate < today ? { dateInPast: { value: control.value } } : null;
    };
}

// Validator for dateRappelFin
export function dateRappelFinValidator(dateRappelControl: AbstractControl, periodiciteControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!dateRappelControl || !dateRappelControl.value || !periodiciteControl || !periodiciteControl.value) {
            return null;
        }

        if (!dateRappelControl || !dateRappelControl.value) {
            return { dateRappelRequired: { value: control.value } };
        }

        const dateRappel = new Date(dateRappelControl.value);
        const dateRappelFin = new Date(control.value);
        if (dateRappelFin < dateRappel) {
            return { dateBeforeRappel: { value: control.value } };
        }
        return null;
    };
}

// Validator for periodicite
export function periodiciteValidator(dateRappelControl: AbstractControl, dateRappelFinControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        if (control.value !== 'Aucune') {
            if (!dateRappelControl || !dateRappelControl.value || !dateRappelFinControl || !dateRappelFinControl.value) {
                return { datesNotSelected: { value: control.value } };
            }
            const dateRappel = new Date(dateRappelControl.value);
            const dateRappelFin = new Date(dateRappelFinControl.value);
            if (dateRappel > dateRappelFin) {
                return { invalidPeriod: { value: control.value } };
            }
        }
        return null;
    };
}

