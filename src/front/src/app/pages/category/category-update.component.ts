import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryService } from 'src/app/services/category.service';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryModel } from './models/category.model';

@Component({
  selector: 'app-category-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule],
  templateUrl: './category-update.component.html',
})

export class CategoryUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  categoryModel!: CategoryModel;
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: CategoryModel,
    public dialog: MatDialog,
    private categorySevice: CategoryService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<CategoryUpdateComponent>) {

    super();
    this.categoryModel = data;

    this.validationMessages = {
      description: {
        required: 'Informe a descrição da categoria.',
        minlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
        maxlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
      }
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      description: new FormControl(this.categoryModel.description, [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
    });
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  submit() {
    if (!this.form.valid) return;

    this.submitted = true
    this.categoryModel = this.form.value;
    this.categoryModel.categoryId = this.data.categoryId;
    this.categoryModel.type = this.data.type;

    this.categorySevice.update(this.categoryModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar a categoria.');
            return;
          }

          this.toastr.success('Categoria alterada com sucesso.');
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
