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
  NgApexchartsModule,
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexStroke,
  ApexMarkers,
  ApexYAxis,
  ApexGrid,
  ApexTitleSubtitle,
  ApexLegend,
  ApexTooltip
} from "ng-apexcharts";
import { GenerateMontsToFilter } from 'src/app/utils/generate-monts-to-filter';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TransactionYearEvolutionGraphModel } from './models/transaction-year-evolution-graph';

export type BarChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  xaxis: ApexXAxis;
  tooltip: ApexTooltip;
};

export type PieChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

export type LineChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
  tooltip: ApexTooltip;
};

@Component({
  selector: 'app-transaction-category-graph',
  standalone: true,
  imports: [MatCardModule, MaterialModule, CommonModule, NgApexchartsModule],
  templateUrl: './transaction-category-graph.component.html',
  styleUrl: './transaction-category-graph.component.scss',
  providers: [CurrencyPipe]
})
export class TransactionCategoryGraphComponent implements OnInit, OnDestroy {
  @ViewChild("chart") chart: ChartComponent;
  public transactionAmountChart: Partial<BarChartOptions>;
  public transactionQuantityChart: Partial<PieChartOptions>;
  public evolutionYearChart: Partial<LineChartOptions>;

  destroy$: Subject<boolean> = new Subject<boolean>();
  monthModel: MonthModel[];
  categoryTransactionModel: CategoryTransactionGraphModel[];
  transactionEvolutionModel: TransactionYearEvolutionGraphModel[];
  selectedMonth: any;
  fillMonths: GenerateMontsToFilter;
  showValues = false;

  constructor(private dashboardSevice: DashboardService,
    private toastr: ToastrService,
    private currency: CurrencyPipe) {
    this.fillMonths = new GenerateMontsToFilter();
    this.monthModel = this.fillMonths.fillMonthsToFilter(new Date());
    this.selectedMonth = this.monthModel[2]?.referenceDate;
  }

  ngOnInit(): void {
    this.getTransactionGraphData(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  toggleVisibility() {
    this.showValues = !this.showValues;
  }

  getTransactionGraphData(event: MatSelectChange | null) {
    let selectedDate: Date = new Date();

    if (event) {
      selectedDate = event.value;
    }

    this.getTransactionCategoryGraph(selectedDate);
    this.getTransactionEvolutionGraph(selectedDate);
  }

  getTransactionCategoryGraph(selectedDate: Date) {
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
            dataLabels: { enabled: false, formatter: (val: number) => this.currency.transform(val)?.toString() ?? 'R$ -' },
            tooltip: {
              y: {
                formatter: (val: number) => this.currency.transform(val)?.toString() ?? 'R$ -'
              }
            },
            xaxis: {
              categories: this.categoryTransactionModel.map(item => item.categoryDescription),
              labels: {
                formatter: (val) => this.currency.transform(val)?.toString() ?? 'R$ -'
              }
            }
          };

          this.transactionQuantityChart = {
            series: this.categoryTransactionModel.map(item => item.quantity),
            chart: { width: 380, type: "pie" },
            labels: this.categoryTransactionModel.map(item => item.categoryDescription),
            responsive: [
              {
                breakpoint: 480,
                options: {
                  chart: {
                    width: 200
                  },
                  legend: {
                    position: "bottom"
                  }
                }
              }
            ]
          };
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  getTransactionEvolutionGraph(selectedDate: Date) {
    this.dashboardSevice.getTransactionInYearEvolution(selectedDate)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.transactionEvolutionModel = response;

          let maxIncome: number = Math.max(...this.transactionEvolutionModel.map(item => item.totalIncome)) + 1000;
          let minExpense: number = Math.min(...this.transactionEvolutionModel.map(item => item.totalExpense));
          let minBalance: number = Math.min(...this.transactionEvolutionModel.map(item => item.totalBalance));
          let minimalValue: number = (minExpense < minBalance ? minExpense : minBalance) - 1000;

          console.log(maxIncome);
          console.log(minExpense);
          console.log(minBalance);
          console.log(minimalValue);

          this.evolutionYearChart = {
            series: [
              {
                name: "Receitas",
                data: this.transactionEvolutionModel.map(item => item.totalIncome)
              },
              {
                name: "Despesas",
                data: this.transactionEvolutionModel.map(item => item.totalExpense)
              },
              {
                name: "Saldo Mensal",
                data: this.transactionEvolutionModel.map(item => item.totalBalance)
              }
            ],
            chart: {
              height: 350,
              type: "line",
              dropShadow: {
                enabled: true,
                color: "#000",
                top: 18,
                left: 7,
                blur: 10,
                opacity: 0.2
              },
              toolbar: {
                show: false
              }
            },
            colors: ["#0064E8", "#FF0909", "#005422"],
            dataLabels: {
              enabled: false
            },
            stroke: {
              curve: "smooth"
            },
            title: {
              text: "Legendas",
              align: "left"
            },
            grid: {
              borderColor: "#e7e7e7",
              row: {
                colors: ["#f3f3f3", "transparent", "transparent"],
                opacity: 0.5
              }
            },
            markers: {
              size: 1
            },
            tooltip: {
              y: {
                formatter: (val: number) => this.currency.transform(val)?.toString() ?? 'R$ -'
              }
            },
            xaxis: {
              categories: this.transactionEvolutionModel.map(item => `${item.month}/${item.year}`),
              title: {
                text: "Meses"
              }
            },
            yaxis: {
              title: {
                text: "Totais Realizados"
              },
              min: minimalValue,
              max: maxIncome
            },
            legend: {
              position: "top",
              horizontalAlign: "right",
              floating: true,
              offsetY: -25,
              offsetX: -5
            }
          };
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }
}
