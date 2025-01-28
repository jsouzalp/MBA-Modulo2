import { Component, OnDestroy, OnInit } from '@angular/core';
import { CardSumaryModel } from './models/card-sumary.model';
import { DashboardService } from 'src/app/services/dashboard.service';
import { Subject, takeUntil } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { MonthModel } from '../../../models/month-model';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatSelectChange } from '@angular/material/select';
import { GenerateMontsToFilter } from 'src/app/utils/generate-monts-to-filter';

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
  fillMonths: GenerateMontsToFilter;
  
  constructor(private dashboardSevice: DashboardService,
    private toastr: ToastrService) { 
      this.fillMonths = new GenerateMontsToFilter();
      this.monthModel = this.fillMonths.fillMonthsToFilter(new Date());
      this.selectedMonth = this.monthModel[2]?.referenceDate;
    }

  ngOnInit(): void {
    this.monthModel = this.fillMonths.fillMonthsToFilter(new Date());
    this.selectedMonth = this.monthModel[2]?.referenceDate;
    this.getResumeSumary(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  toggleVisibility() {
    this.showValues = !this.showValues;
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

          // https://fonts.google.com/icons
          this.cardSummaryItems = [
            { title: 'Saldo Acumulado Anterior', value: this.cardSumaryModel.walletBalance, icon: 'attach_money' },
            { title: 'Receitas Realizadas', value: this.cardSumaryModel.totalIncome, icon: 'trending_up' },
            { title: 'Despesas Realizadas', value: this.cardSumaryModel.totalExpense, icon: 'trending_down' },
            { title: 'Receitas Hoje', value: this.cardSumaryModel.totalIncomeToday, icon: 'today' },
            { title: 'Despesas Hoje', value: this.cardSumaryModel.totalExpenseToday, icon: 'today' },
            { title: 'Saldo Realizado no Mês', value: this.cardSumaryModel.totalBalance, icon: 'account_balance_wallet' },
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
