import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormControlName,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { MatButtonModule } from '@angular/material/button';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-side-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  templateUrl: './side-login.component.html',
})
export class AppSideLoginComponent extends FormBaseComponent implements OnInit, AfterViewInit,  OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  
  email: string;
  form: FormGroup = new FormGroup({});

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils) {
    super();

    this.validationMessages = {
      email: {
        required: 'Informe o email.',
        email: 'E-mail inválido'
      },
      password: {
        required: 'Informe a senha',
      }
    };

    super.configureMensagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.email = this.localStorageUtils.getEmail();

    this.form = new FormGroup({
      email: new FormControl(this.email, [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });
  }
  
  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }


  get f() {
    return this.form.controls;
  }
  get getEmail() {
    return this.f['email'];
  }

  submit() {
    console.log(this.form.value);
    this.localStorageUtils.setUserToken('token');
    this.localStorageUtils.setEmail(this.getEmail.value);
    this.router.navigate(['/pages/dashboard']);
  }

  ngOnDestroy(): void {
    //throw new Error('Method not implemented.');
  }
}
