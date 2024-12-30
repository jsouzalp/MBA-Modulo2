import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CategoryService } from 'src/app/services/category.service';
import { CategoryModel } from './models/category.model';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { CategoryTypeDescriptions, CategoryTypeEnum } from './enums/category-type.enum';


@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, MatTableModule],
  templateUrl: './category-list.component.html',
})


export class CategoryListComponent implements OnInit, OnDestroy {
  categoryModel: CategoryModel[];
  displayedColumns: string[] = ['Description', 'Type'];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private categorySevice: CategoryService, private toastr: ToastrService) {}

 
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
  
  ngOnDestroy(): void {

  }

}


//private userSevice: UserService, private toastr: ToastrService