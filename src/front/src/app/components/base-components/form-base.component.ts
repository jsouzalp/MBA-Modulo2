import { ElementRef } from '@angular/core';
import { FormGroup, UntypedFormGroup } from '@angular/forms';

import { Observable, fromEvent, merge } from 'rxjs';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';



export abstract class FormBaseComponent {

    displayMessage: DisplayMessage = {};
    genericValidator!: GenericValidator;
    validationMessages!: ValidationMessages;
    dateLogged!: Date;

    mudancasNaoSalvas!: boolean;


    protected configureMensagesValidation(validationMessages: ValidationMessages) {
        this.genericValidator = new GenericValidator(validationMessages);
    }

    protected configureValidationFormBase(
        formInputElements: ElementRef[],
        formGroup: UntypedFormGroup) {

        let controlBlurs: Observable<any>[] = formInputElements
            .map((formControl: ElementRef) =>
                fromEvent(formControl.nativeElement, 'blur'));

        merge(...controlBlurs).subscribe(() => {
            this.validateForm(formGroup)
        });
    }

    protected validateForm(formGroup: UntypedFormGroup) {
        this.displayMessage = this.genericValidator.processarMensagens(formGroup);
        this.mudancasNaoSalvas = true;
    }

    protected delay(ms: number) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    protected handleApiErrors(fail: any): string[] {
        let errors: string[] = [];
        if (fail.status == 400 || fail.status == 409) {
            if (fail.error.message) {
                errors.push(fail.error.message);
                return errors;
            }

            let validationErrorDictionary = JSON.parse(JSON.stringify(fail.error.errors));
            for (var fieldName in validationErrorDictionary) {
                if (validationErrorDictionary.hasOwnProperty(fieldName)) {
                    errors.push(validationErrorDictionary[fieldName]);
                }
            }
        } else {
            console.log(fail.error.errors);
            errors.push('Algo deu errado.');
        }
        return errors;

    }

    protected passwordsMatch(group: FormGroup) {
        const password = group.get('password')?.value;
        const confirmPasswordControl = group.get('confirmPassword');
        
        if (password !== confirmPasswordControl?.value) {
          confirmPasswordControl?.setErrors({ notMatching: true });
        } else {
          confirmPasswordControl?.setErrors(null);
        }
    
        return password === confirmPasswordControl?.value ? null : { notMatching: true };
      }

}