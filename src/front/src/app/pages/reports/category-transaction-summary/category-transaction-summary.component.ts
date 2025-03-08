import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material.module';
import { Subject, takeUntil } from 'rxjs';
import { ReportCategory } from './models/transaction.models';
import { ReportCategoryService } from 'src/app/services/report-category.service';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-transaction-summary',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './category-transaction-summary.component.html',
  styleUrl: './category-transaction-summary.component.scss'
})
export class CategoryTransactionSummaryComponent implements OnInit, OnDestroy {


  @Input() startDate!: Date | null;
  @Input() endDate!: Date | null;

  reportcategoryModel: ReportCategory[];
  destroy$: Subject<boolean> = new Subject<boolean>();

  desktop: boolean = true;
  showContent: boolean = false;

  constructor(
    private reportcategoryService: ReportCategoryService,
    private toastr: ToastrService) {
  }

  displayedColumns: string[] = ['transactionDate', 'description', 'totalAmount', 'type'];

  ngOnInit(): void {
  }

  getCategoriesReport(startDt: Date, endDt: Date) {
    this.reportcategoryModel = [];
    this.reportcategoryService.getSummary(startDt, endDt)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.reportcategoryModel = response;
          this.showContent = true
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
          this.showContent = false
        }
      });
  }

  getTransactionSums(): { key: string; value: number }[] {
    const sums: { [key: string]: number } = {};
  
    if (!this.reportcategoryModel) return [];
  
    this.reportcategoryModel.forEach((category: ReportCategory) => {
      const amount = parseFloat(category.totalAmount.replace('R$', '').replace('.', '').replace(',', '.')) || 0;
  
      if (!sums[category.type]) {
        sums[category.type] = 0;
      }
      sums[category.type] += amount;
    });
  
    return Object.entries(sums)
      .map(([key, value]) => ({ key, value })) 
      .sort((b, a) => a.key.localeCompare(b.key)); 
  }

  getTotalAmount(): number {
    let total = 0;
  
    this.getTransactionSums().forEach(transaction => {
      if (transaction.key.toLowerCase() === 'receitas') {
        total += transaction.value; 
      } else if (transaction.key.toLowerCase() === 'despesas') {
        total -= transaction.value; 
      }
    });
  
    return total;
  }

 




  generatePDF(): void {
    this.reportcategoryService.getPdfSummary(this.startDate, this.endDate)
      .subscribe({
        next: (response) => {
          if (response.body) {
            const fileUrl = URL.createObjectURL(response.body);
            if (this.desktop) {
              window.open(fileUrl, '_blank');
            } else {
              var a = document.createElement("a");
              a.href = fileUrl;
              a.download = this.obterNomeArquivo(response);
              a.click();
            }
          }
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      })
  }

  generateExcel(): void {
    this.reportcategoryService.getXlsxSummary(this.startDate, this.endDate)
      .subscribe({
        next: (response) => {
          if (response.body) {
            const fileUrl = URL.createObjectURL(response.body);
            if (this.desktop) {
              window.open(fileUrl, '_blank');
            } else {
              var a = document.createElement("a");
              a.href = fileUrl;
              a.download = this.obterNomeArquivo(response);
              a.click();
            }
          }
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      })
  }

  private obterNomeArquivo(res: HttpResponse<Blob>): string {
    let fileName = 'Relatorio.pdf';
    try {
      if (res.headers.get('content-disposition')) {
        const re_aspas = /\"/gi;
        fileName = res.headers.get('content-disposition')?.split(';')[1].replace("filename=", '').replace(re_aspas, '').trim() ?? 'Relatorio.pdf';
        if (!fileName)
          fileName = 'Relatorio.pdf';
      }
    } catch { }
    return fileName;
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
