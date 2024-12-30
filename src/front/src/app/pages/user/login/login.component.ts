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
import { UserService } from 'src/app/services/user.service';
import { LoginModel } from '../models/login.model';
import { Subject, takeUntil } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  email: string;
  form: FormGroup = new FormGroup({});
  loginModel!: LoginModel;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils, private loginSevice: UserService, private toastr: ToastrService) {
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

    super.configureMessagesValidation(this.validationMessages);
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

    this.loginModel = this.form.value;

    this.loginSevice.login(this.loginModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.localStorageUtils.setUser(response);
          this.router.navigate(['/pages/dashboard']);
        },
        error: (fail) => {
          this.processFail(fail);
        }
      });

  }

  processFail(fail: any) {
    console.log('fail', fail);
    this.form.patchValue({ password: '' });
    this.toastr.error(fail.error.errors);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
