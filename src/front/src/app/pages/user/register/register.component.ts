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
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { UserRegisterModel } from '../models/user-register.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, MaterialModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class UserRegisterComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  form: FormGroup;
  userRegisterModel: UserRegisterModel;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private fb: FormBuilder, private localStorageUtils: LocalStorageUtils, private userSevice: UserService, private toastr: ToastrService) {
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
        name: new FormControl('', [Validators.required, Validators.minLength(6)]),
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

    this.userRegisterModel = this.form.value;

    this.userSevice.register(this.userRegisterModel)
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
