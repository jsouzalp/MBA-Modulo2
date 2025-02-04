import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { TransactionCategoryGraphComponent } from 'src/app/pages/dashboard/transaction-category-graph/transaction-category-graph.component';
import { BalanceCardComponent } from './balance-card/balance-card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule,
    BalanceCardComponent,
    TransactionCategoryGraphComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent { }
