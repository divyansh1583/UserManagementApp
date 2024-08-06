import { AbstractControl, FormControl, ValidatorFn } from "@angular/forms";

export class CustomValidators {
    static noWhitespace(control: AbstractControl): { [key: string]: boolean } | null {
        const isWhitespace = (control.value || '').trim() === '';
        const isValid = !isWhitespace;
        return isValid ? null : { 'whitespace': true };
    }
}
