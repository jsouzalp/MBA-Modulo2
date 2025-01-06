import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CategoryListComponent } from './category/category-list.component';

import { TransactionCreateComponent } from './transaction/transaction-create.component';
import { BudgetByCategoryListComponent } from './budget/by-category/budget-by-category-list.component';
import { GeneralBudgetListComponent } from './budget/general/general-budget-list.component';

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
    ],
  },
];
