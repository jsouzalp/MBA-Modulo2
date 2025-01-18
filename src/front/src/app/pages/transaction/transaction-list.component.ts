import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { CategoryTypeDescriptions, CategoryTypeEnum } from '../category/enums/category-type.enum';
import { TransactionAddComponent } from './transaction-add.component';
import { TransactionUpdateComponent } from './transaction-update.component';
import { TransactionService } from 'src/app/services/transaction.service';
import { GeneralBudgetService } from 'src/app/services/general-budget.service';
import { GeneralBudgetModel } from '../budget/models/general-budget.model';
import { TransactionListModel } from './models/transaction-list.model';
import { TransactionModel } from './models/transaction.model';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss'],
  providers: [CurrencyPipe]
})
export class TransactionListComponent implements OnInit, OnDestroy {
  transactionsModel: TransactionListModel[] = [];
  budgetModel: GeneralBudgetModel;
  displayedColumns: string[] = ['transactionDate', 'description', 'type', 'amount', 'Menu'];
  monthYears: string[] = [];
  selectedMonthYear: string;
  months = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
  balanceDate: Date = new Date();
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private transactionService: TransactionService,
    private generalBudgetService: GeneralBudgetService,
    private toastr: ToastrService,
    public dialog: MatDialog,
    private currencyPipe: CurrencyPipe) { }


  ngOnInit(): void {
    this.getGeneralBudget();
    this.populateMonthYears();
    this.getTransactions();
  }

  getTransactions() {
    this.transactionService.getBalanceByMonth(this.balanceDate)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.transactionsModel = response;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  getGeneralBudget() {
    this.generalBudgetService.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          if (response && response.length > 0) {
            this.budgetModel = response[0];
          }
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  getDescription(type: CategoryTypeEnum): string {
    return CategoryTypeDescriptions[type] || 'Unknown';
  }

  addDialog() {
    const dialogRef = this.dialog.open(TransactionAddComponent, {
      width: '800px',
      height: '440px',
      disableClose: true,
      data: this.transactionsModel
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getTransactions();
        }
      })
  }

  updateDialog(row: any) {

    let transaction: TransactionModel = {
      categoryId: row.categoryId,
      description: row.description,
      transactionId: row.transactionId,
      userId: row.userId,
      amount: row.amount,
      transactionDate: row.transactionDate,
      type: row.type,
    };

    const dialogRef = this.dialog.open(TransactionUpdateComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
      data: transaction
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.updated) {
          this.getTransactions();
        }
      })
  }

  deleteTransaction(transaction: TransactionModel) {

    const dialogData = new ConfirmDialogModel('Atenção', `Confirma exclusão da transação <b>${transaction.description} [${transaction.amount}]</b>?`);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(dialogResult => {
        if (!dialogResult) return;

        this.transactionService.delete(transaction.transactionId)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.toastr.success('Excluída com sucesso.');
              this.getTransactions();
            },
            error: (fail) => {
              this.toastr.error(fail.error.errors);
            }
          });
      });
  }

  getBalance(): number {
    return this.transactionsModel.reduce((acc, item) => acc + item.amount, 0);
  }

  getIncomingBalance(): number {
    return this.transactionsModel
      .filter(item => item.type === 1)
      .reduce((acc, item) => acc + item.amount, 0); // Sum the amounts of filtered items
  }

  getOutBalance(): number {
    return -this.transactionsModel
      .filter(item => item.type === 2)
      .reduce((acc, item) => acc + item.amount, 0); // Sum the amounts of filtered items

  }

  getCategoryBalance(transaction: TransactionListModel): string {
    if (transaction.type === CategoryTypeEnum.Income ) return "";

    if (transaction.categoryBalance < 0) return "Previsão orçamentária não configurada para essa categoria";

    return `Saldo da categoria(${transaction.category}): ${this.currencyPipe.transform(transaction.categoryBalance)}`;
  }

  getGeneralBudgetBalance(): string {
    if (!this.budgetModel || (this.budgetModel.amount === 0 && this.budgetModel.percentage === 0)) return "";

    // Fetch the current balance and incoming balance
    const usedBudget = this.getOutBalance();
    const incomingBalance = this.getIncomingBalance();

    // Determine the budget amount
    const budgetAmount = this.budgetModel.amount || (incomingBalance * (this.budgetModel.percentage / 100));

    // Calculate the used budget percentage
    const usedPercentage = (usedBudget / budgetAmount) * 100;

    const percentageString = usedPercentage > 0
      ? `Orçamento usado ${usedPercentage.toFixed(2)}%. Orçamento ${this.currencyPipe.transform(budgetAmount)} Receitas ${this.currencyPipe.transform(incomingBalance)} Dispesas ${this.currencyPipe.transform(usedBudget)}`
      : `Sem orçamento usado.`;

    return percentageString;
  }

  populateMonthYears(): void {
    const currentYear = new Date().getFullYear();
    const currentMonth = new Date().getMonth();

    for (let year = currentYear + 1; year >= currentYear - 5; year--) {
      for (let i = this.months.length - 1; i >= 0; i--) {
        if (year === currentYear + 1 && i > currentMonth) continue; // Skip future months of the next year
        this.monthYears.push(`${this.months[i]}-${year}`);
      }
    }

    const todayMonthYear = `${this.months[currentMonth]}-${currentYear}`;
    if (this.monthYears.includes(todayMonthYear)) {
      this.selectedMonthYear = todayMonthYear;
    } else if (this.monthYears.length > 0) {
      this.selectedMonthYear = this.monthYears[0];
    }
  }

  onMonthYearSelect(selectedMonthYear: string): void {
    const [month, year] = selectedMonthYear.split('-');
    const monthIndex = this.getMonthIndex(month);
    this.balanceDate = new Date(parseInt(year, 10), monthIndex, 1);
    this.getTransactions();
  }

  getMonthIndex(month: string): number {
    return this.months.indexOf(month);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
