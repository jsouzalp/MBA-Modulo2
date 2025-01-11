import { Component, OnDestroy, OnInit } from '@angular/core';
import { CardSumaryModel } from './models/card-sumary.model';
import { DashboardService } from 'src/app/services/dashboard.service';
import { Subject, takeUntil } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { MonthModel } from '../models/month-model';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-balance-card',
  standalone: true,
  imports: [MatCardModule, MaterialModule, CommonModule],
  templateUrl: './balance-card.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class BalanceCardComponent implements OnInit, OnDestroy {
  cardSumaryModel: CardSumaryModel;
  monthModel: MonthModel[];
  cardSummaryItems: any;
  selectedMonth: any;
  showValues = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private dashboardSevice: DashboardService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.fillMonthsToFilter(new Date());
    this.getResumeSumary(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  toggleVisibility() {
    this.showValues = !this.showValues;
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

  getResumeSumary(event: MatSelectChange | null) {
    let selectedDate: Date = new Date();

    if (event) {
      selectedDate = event.value;
    }

    this.dashboardSevice.getCardSumary(selectedDate)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.cardSumaryModel = response;

          this.cardSummaryItems = [
            { title: 'Saldo da Carteira', value: this.cardSumaryModel.totalBalance, icon: 'account_balance_wallet' },
            { title: 'Receitas Realizadas', value: this.cardSumaryModel.totalIncome, icon: 'trending_up' },
            { title: 'Despesas Realizadas', value: this.cardSumaryModel.totalExpense, icon: 'trending_down' },
            { title: 'Receitas Hoje', value: this.cardSumaryModel.totalIncomeToday, icon: 'today' },
            { title: 'Despesas Hoje', value: this.cardSumaryModel.totalExpenseToday, icon: 'today' },
            { title: 'Receitas Futuras', value: this.cardSumaryModel.futureTotalIncome, icon: 'forward' },
            { title: 'Despesas Futuras', value: this.cardSumaryModel.futureTotalExpense, icon: 'forward' },
          ];      
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }
}
