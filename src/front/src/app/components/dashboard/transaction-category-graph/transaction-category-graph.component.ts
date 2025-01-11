import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { DashboardService } from 'src/app/services/dashboard.service';
import { MonthModel } from '../models/month-model';
import { MatSelectChange } from '@angular/material/select';
import { CategoryTransactionGraphModel } from './models/transaction-category-graph';

@Component({
  selector: 'app-transaction-category-graph',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-category-graph.component.html',
  styleUrl: './transaction-category-graph.component.scss'
})

export class TransactionCategoryGraphComponent implements OnInit, OnDestroy {
  destroy$: Subject<boolean> = new Subject<boolean>();
  monthModel: MonthModel[];
  categoryTransactionModel : CategoryTransactionGraphModel[];
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


          console.log(this.categoryTransactionModel);
          // this.cardSummaryItems = [
          //   { title: 'Saldo da Carteira', value: this.cardSumaryModel.totalBalance, icon: 'account_balance_wallet' },
          //   { title: 'Receitas Realizadas', value: this.cardSumaryModel.totalIncome, icon: 'trending_up' },
          //   { title: 'Despesas Realizadas', value: this.cardSumaryModel.totalExpense, icon: 'trending_down' },
          //   { title: 'Receitas Hoje', value: this.cardSumaryModel.totalIncomeToday, icon: 'today' },
          //   { title: 'Despesas Hoje', value: this.cardSumaryModel.totalExpenseToday, icon: 'today' },
          //   { title: 'Receitas Futuras', value: this.cardSumaryModel.futureTotalIncome, icon: 'forward' },
          //   { title: 'Despesas Futuras', value: this.cardSumaryModel.futureTotalExpense, icon: 'forward' },
          // ];      
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }  
}
