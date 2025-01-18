import { Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryService } from 'src/app/services/category.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BudgetModel } from '../models/budget.model';
import { BudgetService } from 'src/app/services/budget.service';
import { CategoryModel } from '../../category/models/category.model';
import { NgxCurrencyDirective } from 'ngx-currency';


@Component({
  selector: 'app-budget-add',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, MatButtonModule, NgxCurrencyDirective],
  templateUrl: './budget-by-category-add.component.html',
})

export class BudgetByCategoryAddComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  budgetModel!: BudgetModel;
  categoryModel!: CategoryModel[];
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(public dialog: MatDialog,
    private budgetSevice: BudgetService,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<BudgetByCategoryAddComponent>) {

    super();

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
      amount: new FormControl<number | null>(null, [Validators.required, Validators.min(0.01)]),
      categoryId: new FormControl('', [Validators.required]),
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
    if (!this.form.valid) return;
    
    this.submitted = true
    this.budgetModel = this.form.value;
    this.budgetSevice.create(this.budgetModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar o limite orçamentário.');
            return;
          }

          this.toastr.success('Limite orçamentário para a categoria criado com sucesso.');
          this.dialogRef.close({ inserted: true })
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
