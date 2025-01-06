import { Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AbstractControl, FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { GeneralBudgetService } from 'src/app/services/general-budget.service';
import { GeneralBudgetModel } from '../models/general-budget.model';
import { NgxCurrencyDirective } from 'ngx-currency';


@Component({
  selector: 'app-general-budget-add',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, MatButtonModule, NgxCurrencyDirective],
  templateUrl: './general-budget-add.component.html',
})

export class GeneralBudgetAddComponent extends FormBaseComponent implements OnInit, OnDestroy {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  budgetModel!: GeneralBudgetModel;
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(public dialog: MatDialog,
    private budgetSevice: GeneralBudgetService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<GeneralBudgetAddComponent>) {

    super();

    this.validationMessages = {
      amount: {},
      percentage: {},
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = new FormGroup(
      {
        amount: new FormControl<number | null>(null),
        percentage: new FormControl<number | null>(null),
      },
      { validators: this.amountOrPercentageValidator }
    );
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  amountOrPercentageValidator(group: AbstractControl): ValidationErrors | null {
    const amount = group.get('amount')?.value;
    const percentage = group.get('percentage')?.value;

    if ((!amount && !percentage) || (amount == 0 && percentage == 0)) {
      return { requiredOne: 'Informe o valou ou o porcentual.' };
    }

    if (amount && percentage) {
      return { onlyOneAllowed: 'Informe o valou ou o porcentual.' };
    }

    if (percentage && (percentage < 1 || percentage > 100)) {
      return { percentageRange: 'O porcentual deve estar entre 1 e 100.' };
    }

    if (amount && amount <= 0) {
      return { amountRange: 'O valor deve ser maior que zero.' };
    }

    return null; // Valid
  }

  onAmountChange(): void {
    this.form.get('percentage')?.setValue(null);
  }

  onPercentageChange(): void {
    this.form.get('amount')?.setValue(null);
  }

  submit() {
    this.budgetModel = this.form.value;
    this.budgetSevice.create(this.budgetModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar a categoria.');
            return;
          }

          let toast = this.toastr.success('Categoria criada com sucesso.');
          if (toast) {
            toast.onHidden.pipe(takeUntil(this.destroy$)).subscribe(() => {
              this.dialogRef.close({ inserted: true })
            });
          }

        },
        error: (fail) => {
          this.submitted = false;
          this.toastr.error(fail.error.errors);
        }
      });
  }

  cancel() {
    this.dialogRef.close({ inserted: false });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
