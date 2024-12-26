import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CategoryListComponent } from './category/category-list.component';
import { BudgetByCategoryComponent } from './budget/budget-by-category.component';
import { BudgetGeneralComponent } from './budget/budget-general.component';
import { TransactionCreateComponent } from './transaction/transaction-create.component';

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
        component: BudgetByCategoryComponent,
      },      
      {
        path: 'budget/general',
        component: BudgetGeneralComponent,
      },
      {
        path: 'transaction/create',
        component: TransactionCreateComponent,
      }, 
    ],
  },
];
