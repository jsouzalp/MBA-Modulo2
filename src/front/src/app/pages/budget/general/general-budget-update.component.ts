import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AbstractControl, FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { CategoryService } from 'src/app/services/category.service';
import { CategoryModel } from '../../category/models/category.model';
import { GeneralBudgetService } from 'src/app/services/general-budget.service';
import { GeneralBudgetModel } from '../models/general-budget.model';
import { NgxCurrencyDirective } from 'ngx-currency';


@Component({
  selector: 'app-general-budget-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, NgxCurrencyDirective],
  templateUrl: './general-budget-update.component.html',
})

export class GeneralBudgetUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  budgetModel!: GeneralBudgetModel;
  categoryModel!: CategoryModel[];
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: GeneralBudgetModel,
    public dialog: MatDialog,
    private budgetSevice: GeneralBudgetService,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<GeneralBudgetUpdateComponent>) {

    super();
    this.budgetModel = data;

    this.validationMessages = {
      amount: {
        requiredOne: 'Informe o valor ou o porcentual.',
        min: 'O valor deve ser maior que zero.',
        onlyOneAllowed: 'Informe o valor ou o porcentual.'
      },
      percentage: {
        requiredOne: 'Informe o valor ou o porcentual.',
        min: 'O valor deve ser maior que zero.',
        max: 'O valor não pode ultrapassar 100%.',
        onlyOneAllowed: 'Informe o valor ou o porcentual.'
      },
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.getCategories();

    this.form = new FormGroup(
      {
        amount: new FormControl<number | null>(this.data.amount),
        percentage: new FormControl<number | null>(this.data.percentage),
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
      return { requiredOne: 'Informe apenas o valor ou o porcentual.' };
    }

    if (amount && percentage) {
      return { onlyOneAllowed: 'Informe apenas o valor ou o porcentual.' };
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

  getCategories() {
    this.categorySevice.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.categoryModel = response;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  submit() {
    if (!this.form.valid) return;
    
    this.submitted = true;
    this.budgetModel = this.form.value;
    this.budgetModel.generalBudgetId = this.data.generalBudgetId;

    this.budgetSevice.update(this.budgetModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar o limite orçamentário geral.');
            return;
          }

          this.toastr.success('Limite orçamentário geral alterado com sucesso.');
          this.dialogRef.close({ updated: true })
        },
        error: (fail) => {
          this.submitted = false;
          this.toastr.error(fail.error.errors);
        }
      });
  }

  cancel() {
    this.dialogRef.close({ updated: false });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
