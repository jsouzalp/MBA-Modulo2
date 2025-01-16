import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CategoryListComponent } from './category/category-list.component';

import { TransactionCreateComponent } from './transaction/transaction-create.component';
import { BudgetByCategoryListComponent } from './budget/by-category/budget-by-category-list.component';
import { GeneralBudgetListComponent } from './budget/general/general-budget-list.component';
import { CategoryTransactionSummaryComponent } from './reports/category-transaction-summary/category-transaction-summary.component';
import { CategoryTransactionAnalyticsComponent } from './reports/category-transaction-analytics/category-transaction-analytics.component';
import { CategoryTransactionComponent } from './reports/category-transaction/category-transaction.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'category/list',
        component: CategoryListComponent,
      },
      {
        path: 'budget/by-category',
        component: BudgetByCategoryListComponent,
      },      
      {
        path: 'budget/general',
        component: GeneralBudgetListComponent,
      },
      {
        path: 'transaction/create',
        component: TransactionCreateComponent,
      },
      {
        path: 'report/categorySummary',
        component: CategoryTransactionSummaryComponent,
      },
      {
        path: 'report/categoryAnalytics',
        component: CategoryTransactionAnalyticsComponent,
      },
      {
        path: 'report/category',
        component: CategoryTransactionComponent,
      }
    ],
  },
];
