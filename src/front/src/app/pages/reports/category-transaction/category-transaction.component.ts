import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, ViewChild } from '@angular/core';
import { CategoryTransactionSummaryComponent } from '../category-transaction-summary/category-transaction-summary.component';
import { CommonModule } from '@angular/common';
import { CategoryTransactionAnalyticsComponent } from "../category-transaction-analytics/category-transaction-analytics.component";
import { MaterialModule } from 'src/app/material.module';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import * as _moment from 'moment';
import 'moment/locale/pt-br';
import { provideMomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from 'src/app/shared/constants';


@Component({
  selector: 'app-category-transaction',
  standalone: true,
  imports: [FormsModule, CommonModule, MaterialModule, CategoryTransactionSummaryComponent, CategoryTransactionAnalyticsComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    provideMomentDateAdapter(MY_DATE_FORMATS)

  ],
  templateUrl: './category-transaction.component.html',
  styleUrl: './category-transaction.component.scss'
})
export class CategoryTransactionComponent implements OnInit {

  @ViewChild('summary') summaryComponent: CategoryTransactionSummaryComponent;
  @ViewChild('analytics') analyticsComponent: CategoryTransactionAnalyticsComponent;


  constructor(private toastr: ToastrService) { }

  ngOnInit(): void {
    const currentDate = new Date();
  }

  startDateValue: Date;
  endDateValue: Date;

  selectedOption: string | null = 'summary';

  previousOption: string | null = null;

  confirmed: boolean = false;

  onButtonClick(): void {

    if (this.startDateValue > this.endDateValue) {
      this.toastr.error("A data de início não pode ser posterior à data de término.");
      this.confirmed = false;
      return;
    }

    if (!this.selectedOption)
      return;

    this.confirmed = true;

    if (this.selectedOption == 'summary')
      this.summaryComponent.getCategoriesReport(this.startDateValue, this.endDateValue);
    else
      this.analyticsComponent.getCategoriesReport(this.startDateValue, this.endDateValue);
  }

  alterouSelecao() {

    this.confirmed = false;
  }



}




