import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryService } from 'src/app/services/category.service';
import { CategoryModel } from './models/category.model';
import { CommonModule } from '@angular/common';
import { CategoryTypeDescriptions, CategoryTypeEnum } from './enums/category-type.enum';
import { MatDialog } from '@angular/material/dialog';
import { CategoryAddComponent } from './category-add.component';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { CategoryUpdateComponent } from './category-update.component';


@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.conponent.scss'],
})


export class CategoryListComponent implements OnInit, OnDestroy {
  categoryModel: CategoryModel[];
  displayedColumns: string[] = ['description', 'type', 'Menu'];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private categorySevice: CategoryService,
    private toastr: ToastrService,
    public dialog: MatDialog) { }


  ngOnInit(): void {
    this.getCategories();
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

  getDescription(type: CategoryTypeEnum): string {
    return CategoryTypeDescriptions[type] || 'Unknown';
  }

  addDialog() {
    const dialogRef = this.dialog.open(CategoryAddComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
      data: this.categoryModel
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getCategories();
        }
      })
  }

  updateDialog(row:any) {

    let category: CategoryModel = {
      categoryId: row.categoryId,
      description: row.description,
      userId: row.userId,
      type: row.type
    };

    const dialogRef = this.dialog.open(CategoryUpdateComponent, {
      width: '500px',
      height: '300px',
      disableClose: true,
      data: category
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getCategories();
        }
      })
  }


  deleteCategory(category: CategoryModel) {

    const dialogData = new ConfirmDialogModel('Atenção', `Confirma exclusão da categoria <b>${category.description}</b>?`);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(dialogResult => {
        if (!dialogResult) return;

        this.categorySevice.delete(category.categoryId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.toastr.success('Categoria excluída com sucesso.');
              this.getCategories();
            },
            error: (fail) => {
              this.toastr.error(fail.error.errors);
            }
          });
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
