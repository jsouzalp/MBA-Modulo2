import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
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

  @Input() refrash!: number | null;

  reportcategoryModel: ReportCategory[];
  destroy$: Subject<boolean> = new Subject<boolean>();

  desktop: boolean = true;


  constructor(
    private reportcategoryService: ReportCategoryService,
    private toastr: ToastrService) {
    // TODO: verificar se é desktop
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
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }



  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  generatePDF(): void {

    console.log("generateExcel()->" + this.startDate + " - " + this.endDate)

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

    console.log("generateExcel()->" + this.startDate + " - " + this.endDate)

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




}
