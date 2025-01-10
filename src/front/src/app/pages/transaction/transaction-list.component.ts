import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { CategoryTypeDescriptions, CategoryTypeEnum } from '../category/enums/category-type.enum';
import { TransactionAddComponent } from './transaction-add.component';
import { TransactionUpdateComponent } from './transaction-update.component';
import { TransactionModel } from './models/transaction.model';
import { TransactionService } from 'src/app/services/transaction.service';


@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './transaction-list.component.html',
})


export class TransactionListComponent implements OnInit, OnDestroy {
  transactionModels: TransactionModel[] = [];
  displayedColumns: string[] = ['description', 'type', 'amount', 'Menu'];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private transactionService: TransactionService,
    private toastr: ToastrService,
    public dialog: MatDialog) { }


  ngOnInit(): void {
    this.getBudgeties();
  }

  getBudgeties() {
    this.transactionService.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.transactionModels = response;
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
      width: '500px',
      height: '400px',
      data: this.transactionModels
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.inserted) {
          this.getBudgeties();
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
      transactionDate: new Date()
    };

    const dialogRef = this.dialog.open(TransactionUpdateComponent, {
      width: '500px',
      height: '400px',
      data: transaction
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (res.updated) {
          this.getBudgeties();
        }
      })
  }

  deleteCategory(id: string) {

    const dialogData = new ConfirmDialogModel('Atenção', 'Confirma exclusão ?');

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(dialogResult => {
        if (!dialogResult) return;

        this.transactionService.delete(id)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.toastr.success('Excluída com sucesso.');
              this.getBudgeties();
            },
            error: (fail) => {
              this.toastr.error(fail.error.errors);
            }
          });
      });
  }

  getbalance(): number {
    return this.transactionModels.reduce((acc, item) => acc + item.amount, 0);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
