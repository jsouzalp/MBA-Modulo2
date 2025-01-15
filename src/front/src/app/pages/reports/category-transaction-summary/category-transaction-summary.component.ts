import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material.module';
import { Subject, takeUntil } from 'rxjs';
import { ReportCategory } from './models/transaction.models';
import { ReportCategoryService } from 'src/app/services/report-category.service';

@Component({
  selector: 'app-category-transaction-summary',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './category-transaction-summary.component.html',
  styleUrl: './category-transaction-summary.component.scss'
})
export class CategoryTransactionSummaryComponent implements OnInit, OnDestroy {

  reportcategoryModel: ReportCategory[];
  destroy$: Subject<boolean> = new Subject<boolean>();


  constructor(
    private reportcategoryService: ReportCategoryService,
    private toastr: ToastrService) { }

    displayedColumns: string[] = ['transactionDate', 'description', 'totalAmount', 'type'];


  ngOnInit(): void {
    this.getCategoriesReport();
  }


  getCategoriesReport() {
    this.reportcategoryService.getSummary()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.reportcategoryModel = response;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });

      console.log(this.reportcategoryModel);
  }



  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }






}
