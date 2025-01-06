import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BudgetModel } from '../models/budget.model';
import { BudgetService } from 'src/app/services/budget.service';
import { CategoryService } from 'src/app/services/category.service';
import { CategoryModel } from '../../category/models/category.model';
import { NgxCurrencyDirective } from 'ngx-currency';


@Component({
  selector: 'app-budget-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, NgxCurrencyDirective],
  templateUrl: './budget-by-category-update.component.html',
})

export class BudgetUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  budgetModel!: BudgetModel;
  categoryModel!: CategoryModel[];
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: BudgetModel,
    public dialog: MatDialog,
    private budgetSevice: BudgetService,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<BudgetUpdateComponent>) {

    super();
    this.budgetModel = data;

    this.validationMessages = {
      amount: {
        required: 'O valor deve ser maior que zero.',
        min: 'O valor deve ser maior que zero.',
      },
      categoryId: {
        required: 'A categoria é obrigatória.',
      },
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.getCategories();
    this.form = new FormGroup({
      amount: new FormControl<number|null>(this.budgetModel.amount, [Validators.required, Validators.min(0.01)]),
      categoryId: new FormControl(this.budgetModel.categoryId, [Validators.required]),
    });
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
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
    this.budgetModel = this.form.value;
    this.budgetModel.budgetId = this.data.budgetId;
    
    this.budgetSevice.update(this.budgetModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar a categoria.');
            return;
          }

          let toast = this.toastr.success('Categoria alterada com sucesso.');
          if (toast) {
            toast.onHidden.pipe(takeUntil(this.destroy$)).subscribe(() => {
              this.dialogRef.close({ updated: true })
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
    this.dialogRef.close({ updated: false });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
