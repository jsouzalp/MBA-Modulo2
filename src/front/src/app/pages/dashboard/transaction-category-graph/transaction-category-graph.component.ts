import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { DashboardService } from 'src/app/services/dashboard.service';
import { MonthModel } from '../../../models/month-model';
import { MatSelectChange } from '@angular/material/select';
import { CategoryTransactionGraphModel } from './models/transaction-category-graph';

import {
  ApexAxisChartSeries,
  ApexChart,
  ChartComponent,
  ApexDataLabels,
  ApexXAxis,
  ApexPlotOptions,
  NgApexchartsModule
} from "ng-apexcharts";
import { GenerateMontsToFilter } from 'src/app/utils/generate-monts-to-filter';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  xaxis: ApexXAxis;
};

@Component({
  selector: 'app-transaction-category-graph',
  standalone: true,
  imports: [MatCardModule, MaterialModule, CommonModule, NgApexchartsModule],
  templateUrl: './transaction-category-graph.component.html',
  styleUrl: './transaction-category-graph.component.scss'
})

export class TransactionCategoryGraphComponent implements OnInit, OnDestroy {
  @ViewChild("chart") chart: ChartComponent;
  public transactionAmountChart: Partial<ChartOptions>;
  public transactionQuantityChart: Partial<ChartOptions>;

  destroy$: Subject<boolean> = new Subject<boolean>();
  monthModel: MonthModel[];
  categoryTransactionModel: CategoryTransactionGraphModel[];
  selectedMonth: any;
  fillMonths: GenerateMontsToFilter;

  constructor(private dashboardSevice: DashboardService,
    private toastr: ToastrService) { 
      this.fillMonths = new GenerateMontsToFilter();
      this.monthModel = this.fillMonths.fillMonthsToFilter(new Date());
      this.selectedMonth = this.monthModel[2]?.referenceDate;
    }

  ngOnInit(): void {
    
    this.getTransactionCategoryGraph(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  getTransactionCategoryGraph(event: MatSelectChange | null) {
    let selectedDate: Date = new Date();

    if (event) {
      selectedDate = event.value;
    }

    this.dashboardSevice.getTransactionCategorySumary(selectedDate)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.categoryTransactionModel = response;

          this.transactionAmountChart = {
            series: [{
              name: 'Valor Total',
              data: this.categoryTransactionModel.map(item => item.totalAmount),

            }],
            chart: { type: "bar", height: 350 },
            plotOptions: { bar: { horizontal: true } },
            dataLabels: { enabled: false, formatter: (val: number) => `R$ ${val.toFixed(2)}` },
            xaxis: {
              categories: this.categoryTransactionModel.map(item => item.categoryDescription)
            }
          };

          this.transactionQuantityChart = {
            series: [{
              name: 'Quantidade',
              data: this.categoryTransactionModel.map(item => item.quantity),
            }],
            chart: { type: "bar" },
            plotOptions: { bar: { horizontal: true } },
            dataLabels: { enabled: false },
            xaxis: {
              categories: this.categoryTransactionModel.map(item => item.categoryDescription)
            }
          };
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }
}
