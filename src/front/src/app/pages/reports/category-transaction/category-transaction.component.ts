import { Component,CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { CategoryTransactionSummaryComponent } from '../category-transaction-summary/category-transaction-summary.component';
import { CommonModule } from '@angular/common';
import { CategoryTransactionAnalyticsComponent } from "../category-transaction-analytics/category-transaction-analytics.component";
import { MaterialModule } from 'src/app/material.module';
import { FormsModule } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, NativeDateAdapter } from '@angular/material/core';

export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'YYYY/MM/DD', // formato de entrada para o datepicker
  },
  display: {
    dateInput: 'YYYY/MM/DD', // formato de exibição para o input
    monthYearLabel: 'MMM YYYY', // formato de exibição do mês e ano
    dateA11yLabel: 'LL', // formato de exibição para acessibilidade
    monthYearA11yLabel: 'MMMM YYYY', // formato de mês/ano para acessibilidade
  },
};


@Component({
  selector: 'app-category-transaction',
  standalone: true,
  imports: [FormsModule ,CommonModule,MaterialModule, CategoryTransactionSummaryComponent, CategoryTransactionAnalyticsComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter }, 
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' }, 
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
  templateUrl: './category-transaction.component.html',
  styleUrl: './category-transaction.component.scss'
})



export class CategoryTransactionComponent implements OnInit {
  ngOnInit(): void {
    const currentDate = new Date();
    this.startDateValue = currentDate;
    this.endDateValue = currentDate;
  }

  startDateValue: Date | null = null;
  endDateValue: Date | null = null;
  selectedOption: string = 'summary';
  showComponent: boolean = false;
  
  onButtonClick(): void {

    this.showComponent = true;

    console.log(`Data inicial: ${this.startDateValue?.toISOString().split('T')[0]}`);
    console.log(`Data final: ${this.endDateValue?.toISOString().split('T')[0]}`);

  }




}




