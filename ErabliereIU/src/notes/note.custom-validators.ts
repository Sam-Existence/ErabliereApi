import { FormGroup, ValidationErrors } from "@angular/forms";

export function reminderValidator(formGroup: FormGroup): ValidationErrors | null {
    const reminderEnabled = formGroup.get('reminderEnabled')?.value;
    const dateRappel = formGroup.get('dateRappel')?.value;
    const dateRappelFin = formGroup.get('dateRappelFin')?.value;
    const periodicite = formGroup.get('periodicite')?.value;

    let errors: any = {};

    if (reminderEnabled) {
        if (!dateRappel) {
            formGroup.get('dateRappel')?.setErrors({ dateRappelRequired: true });
        } else if (new Date(dateRappel) < new Date()) {
            formGroup.get('dateRappel')?.setErrors({ dateRappelPast: true });
        }

        if (dateRappelFin && !dateRappel) {
            formGroup.get('dateRappel')?.setErrors({ dateRappelRequired: true });
        } else if (dateRappelFin && new Date(dateRappelFin) < new Date(dateRappel)) {
            formGroup.get('dateRappelFin')?.setErrors({ dateRappelFinBeforeDateRappel: true });
        }

        if (periodicite !== 'Aucune' && (!dateRappel || !dateRappelFin)) {
            formGroup.get('periodicite')?.setErrors({ datesRequiredForPeriodicite: true });
        }
    }

    return Object.keys(errors).length > 0 ? errors : null;
}
