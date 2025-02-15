import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material.module';
import { Subject, takeUntil } from 'rxjs';
import { ReportCategoryService } from 'src/app/services/report-category.service';
import { ReportCategoryAnalytics } from './Models/transaction.models';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-transaction-analytics',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './category-transaction-analytics.component.html',
  styleUrl: './category-transaction-analytics.component.scss'
})
export class CategoryTransactionAnalyticsComponent implements OnDestroy {


  @Input() startDate!: Date | null;
  @Input() endDate!: Date | null;


  reportcategoryModel: ReportCategoryAnalytics[];
  destroy$: Subject<boolean> = new Subject<boolean>();

  desktop: boolean = true;
  showContent: boolean = false;

  constructor(
    private reportcategoryService: ReportCategoryService,
    private toastr: ToastrService) { }

  displayedColumns: string[] = ['transactionDate', 'description', 'totalAmount', 'type'];


  getCategoriesReport(startDt: Date, endDt: Date) {

    this.reportcategoryModel = [];
    this.reportcategoryService.getAnalytics(startDt, endDt)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.reportcategoryModel = response;
          this.showContent = true;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
          this.showContent = false;
        }
      });
  }

  // getTransactionSums(): { [key: string]: number } {
  //   const sums: { [key: string]: number } = {};
  
  //   if (!this.reportcategoryModel) return sums;
  
  //   this.reportcategoryModel.forEach(category => {
  //     category.transactions.forEach(transaction => {
  //       const amount = parseFloat(transaction.totalAmount.replace('R$', '').replace('.', '').replace(',', '.')) || 0;
  
  //       if (!sums[transaction.type]) {
  //         sums[transaction.type] = 0;
  //       }
  //       sums[transaction.type] += amount;
  //     });
  //   });
  
  //   return sums;
  // }

  getTransactionSums(): { key: string; value: number }[] {
    const sums: { [key: string]: number } = {};
  
    if (!this.reportcategoryModel) return [];
  
    this.reportcategoryModel.forEach(category => {
      category.transactions.forEach(transaction => {
        const amount = parseFloat(transaction.totalAmount.replace('R$', '').replace('.', '').replace(',', '.')) || 0;
  
        if (!sums[transaction.type]) {
          sums[transaction.type] = 0;
        }
        sums[transaction.type] += amount;
      });
    });
  
    return Object.entries(sums)
      .map(([key, value]) => ({ key, value })) 
      .sort((b, a) => a.key.localeCompare(b.key)); 
  }

  getTotalAmount(): number {
    let total = 0;
    
    this.getTransactionSums().forEach(transaction => {
      if (transaction.key.toLowerCase() === 'receitas') {
        total += transaction.value; // Soma as receitas
      } else if (transaction.key.toLowerCase() === 'despesas') {
        total -= transaction.value; // Subtrai as despesas
      }
    });
  
    return total;
  }
  

  

  generatePDF(): void {
    this.reportcategoryService.getPdfAnalytics(this.startDate, this.endDate)
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
    this.reportcategoryService.getXlsxAnalytics(this.startDate, this.endDate)
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
