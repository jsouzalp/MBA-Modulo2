import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BudgetService } from 'src/app/services/budget.service';
import { CategoryService } from 'src/app/services/category.service';
import { NgxCurrencyDirective } from 'ngx-currency';
import { CategoryModel } from '../category/models/category.model';
import { TransactionService } from 'src/app/services/transaction.service';
import { TransactionModel } from './models/transaction.model';
import { MessageService } from 'src/app/services/message.service ';
import { provideNativeDateAdapter } from '@angular/material/core';


@Component({
  selector: 'app-transaction-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, NgxCurrencyDirective],
  templateUrl: './Transaction-update.component.html',
  providers: [CurrencyPipe, provideNativeDateAdapter()]
})
export class TransactionUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  transactionModel!: TransactionModel;
  categoryModel!: CategoryModel[];
  submitted = false;
  updated = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: TransactionModel,
    public dialog: MatDialog,
    private transactionService: TransactionService,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<TransactionUpdateComponent>,
    private messageService: MessageService) {

    super();
    this.transactionModel = data;

    this.validationMessages = {
      description: {
        required: 'Informe a descrição da categoria.',
        minlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
        maxlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
      },
      amount: {
        required: 'O valor deve ser maior que zero.',
        min: 'O valor deve ser maior que zero.',
      },
      categoryId: {
        required: 'A categoria é obrigatória.',
      },
      transactionDate: {
        required: 'A data da transação é obrigatória.',
      }
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.getCategories();
    this.form = new FormGroup({
      description: new FormControl(this.transactionModel.description, [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      amount: new FormControl<number>(this.transactionModel.amount, [Validators.required, Validators.min(0.01)]),
      categoryId: new FormControl(this.transactionModel.categoryId, [Validators.required]),
      transactionDate: new FormControl(this.transactionModel.transactionDate, [Validators.required]),
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

    this.submitted = true;
    this.transactionModel = this.form.value;
    this.transactionModel.transactionId = this.data.transactionId;

    this.transactionService.update(this.transactionModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar o lançamento.');
            return;
          }

          this.toastr.success('Lançamento efetuado com sucesso.');
          this.updated = true;
          this.cancel();
        },
        error: (fail) => {
          this.submitted = false;
          this.toastr.error(fail.error.errors);
        }
      });
  }

  cancel() {
    this.dialogRef.close({ inserted: this.updated });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
