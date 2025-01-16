import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { GeneralBudgetAddComponent } from './general-budget-add.component';
import { GeneralBudgetUpdateComponent } from './general-budget-update.component';
import { GeneralBudgetService } from 'src/app/services/general-budget.service';
import { GeneralBudgetModel } from '../models/general-budget.model';

@Component({
  selector: 'app-general-budget-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './general-budget-list.component.html',
})


export class GeneralBudgetListComponent implements OnInit, OnDestroy {
  budgetModel: GeneralBudgetModel[] = [];
  displayedColumns: string[] = ['amount', 'percentage', 'Menu'];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private budgetSevice: GeneralBudgetService,
    private toastr: ToastrService,
    public dialog: MatDialog) { }


  ngOnInit(): void {
    this.getBudgeties();
  }

  getBudgeties() {
    this.budgetSevice.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.budgetModel = response;
        },
        error: (fail) => {
          this.toastr.error(fail.error.errors);
        }
      });
  }

  addDialog() {
    const dialogRef = this.dialog.open(GeneralBudgetAddComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
      data: this.budgetModel
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

    let category: GeneralBudgetModel = {
      generalBudgetId: row.generalBudgetId,
      amount: row.amount,
      percentage: row.percentage
    };

    const dialogRef = this.dialog.open(GeneralBudgetUpdateComponent, {
      width: '500px',
      height: '400px',
      disableClose: true,
      data: category
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
      disableClose: true,
      data: dialogData
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(dialogResult => {
        if (!dialogResult) return;

        this.budgetSevice.delete(id)
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

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
