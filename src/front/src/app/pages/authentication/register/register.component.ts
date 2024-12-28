import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormControlName,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, MaterialModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class UserRegisterComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  form: FormGroup;

  constructor(private router: Router, private fb: FormBuilder) {
    super();
    
    this.validationMessages = {
      userName: {
        required: 'Informe o nome.',
        minlength: 'Informe o nome completo.'
      },      
      email: {
        required: 'Informe o email.',
        email: 'E-mail inválido'
      },      
      password: {
        required: 'Informe a senha',
        pattern: 'A senha deve ter entre 8 e 50 caracteres, incluindo números e símbolos.',
      },
      confirmPassword: {
        required: 'Confirme sua senha',
        notMatching: 'As senhas não coincidem.',
      },
    };

    super.configureMessagesValidation(this.validationMessages);

  }

  ngOnInit(): void {

    this.form = this.fb.group(
      {
        userName: new FormControl('', [Validators.required, Validators.minLength(6)]),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/),
        ]),
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordsMatch }
    );

  }
  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  get f() {
    return this.form.controls;
  }

  submit() {
     console.log(this.form);
    this.router.navigate(['/']);
  }

  ngOnDestroy(): void {
   
  }
}
