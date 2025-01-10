import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BudgetService } from 'src/app/services/budget.service';
import { CategoryService } from 'src/app/services/category.service';
import { NgxCurrencyDirective } from 'ngx-currency';
import { CategoryModel } from '../category/models/category.model';
import { TransactionModel } from './models/transaction.model';
import { TransactionService } from 'src/app/services/transaction.service';


@Component({
  selector: 'app-transaction-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, NgxCurrencyDirective],
  templateUrl: './Transaction-update.component.html',
})

export class TransactionUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  transactionModel!: TransactionModel;
  categoryModel!: CategoryModel[];
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: TransactionModel,
    public dialog: MatDialog,
    private transactionService: TransactionService,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<TransactionUpdateComponent>) {

    super();
    this.transactionModel = data;

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
      amount: new FormControl<number|null>(this.transactionModel.amount, [Validators.required, Validators.min(0.01)]),
      categoryId: new FormControl(this.transactionModel.categoryId, [Validators.required]),
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
    this.transactionModel = this.form.value;
    this.transactionModel.transactionId = this.data.transactionId;
    
    this.transactionService.update(this.transactionModel)
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
