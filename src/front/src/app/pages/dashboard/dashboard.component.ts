import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { AppSalesProfitComponent } from 'src/app/components/sales-profit/sales-profit.component';
import { AppTotalFollowersComponent } from 'src/app/components/total-followers/total-followers.component';
import { AppTotalIncomeComponent } from 'src/app/components/total-income/total-income.component';
import { AppPopularProductsComponent } from 'src/app/components/popular-products/popular-products.component';
import { AppEarningReportsComponent } from 'src/app/components/earning-reports/earning-reports.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule,
    AppSalesProfitComponent,
    AppTotalFollowersComponent,
    AppTotalIncomeComponent,
    AppPopularProductsComponent,
    AppEarningReportsComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent {}
