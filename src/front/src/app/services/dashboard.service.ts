import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseService } from './BaseService';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from './message.service ';
import { CategoryTransactionGraphModel } from '../pages/dashboard/transaction-category-graph/models/transaction-category-graph';
import { CardSumaryModel } from '../pages/dashboard/balance-card/models/card-sumary.model';
import { TransactionYearEvolutionGraphModel } from '../pages/dashboard/transaction-category-graph/models/transaction-year-evolution-graph';

@Injectable({ providedIn: 'root' })
export class DashboardService extends BaseService {

  constructor(private http: HttpClient, toastr: ToastrService, messageService: MessageService) {
    super(toastr, messageService);
  }

  getCardSumary(filterDate: Date | null): Observable<CardSumaryModel> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/cards/`;
    if (filterDate) {
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  getTransactionCategorySumary(filterDate: Date | null): Observable<CategoryTransactionGraphModel[]> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/transactions/`;
    if (filterDate) {
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  getTransactionInYearEvolution(filterDate: Date | null): Observable<TransactionYearEvolutionGraphModel[]> {
    let url: string = `${this.UrlServiceV1}v1/dashboard/evolution/`;
    if (filterDate){
      url += this.formatDate(filterDate);
    }

    let response = this.http
      .get(url, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }
}
