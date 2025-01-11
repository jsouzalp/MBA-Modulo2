import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { DashboardService } from 'src/app/services/dashboard.service';
import { MonthModel } from '../models/month-model';
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
  imports: [MaterialModule, NgApexchartsModule],
  templateUrl: './transaction-category-graph.component.html',
  styleUrl: './transaction-category-graph.component.scss'
})

export class TransactionCategoryGraphComponent implements OnInit, OnDestroy {
  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;

  destroy$: Subject<boolean> = new Subject<boolean>();
  monthModel: MonthModel[];
  categoryTransactionModel: CategoryTransactionGraphModel[];
  selectedMonth: any;

  constructor(private dashboardSevice: DashboardService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.fillMonthsToFilter(new Date());
    this.getTransactionCategoryGraph(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  // Isso aqui poderia ficar na service???
  fillMonthsToFilter(nowDate: Date) {
    this.monthModel = [];

    for (let i = -2; i < 12; i++) {
      const date = new Date(nowDate);
      date.setMonth(nowDate.getMonth() - i);

      this.monthModel.push({
        month: date.toLocaleString('pt-BR', { month: 'long', year: 'numeric' }).replace(/^\w/, c => c.toUpperCase()),
        referenceDate: date,
      });
    }
    this.selectedMonth = this.monthModel[2]?.referenceDate;
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

          this.chartOptions = {
            series: [{
              name: 'Total',
              data: this.categoryTransactionModel.map(item => item.totalAmount),
              
            }],            
            chart: { type: "bar", height: 350 },
            plotOptions: { bar: { horizontal: true } },
            dataLabels: { enabled: false, formatter: (val:number) => `R$ ${val.toFixed(2)}` },
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
