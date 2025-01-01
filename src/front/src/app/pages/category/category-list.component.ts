import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryService } from 'src/app/services/category.service';
import { CategoryModel } from './models/category.model';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { CategoryTypeDescriptions, CategoryTypeEnum } from './enums/category-type.enum';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { CategoryAddComponent } from './category-add.component';


@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, MatTableModule, MatIconModule, MatMenuModule, MatButtonModule],
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.conponent.scss'],
})


export class CategoryListComponent implements OnInit, OnDestroy {
  categoryModel: CategoryModel[];
  displayedColumns: string[] = ['Description', 'Type', 'Menu'];
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

  openAddDialog() {
    const dialogRef = this.dialog.open(CategoryAddComponent, {
      width: '500px',
      height: '400px',
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getCategories();
        }
      })
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
