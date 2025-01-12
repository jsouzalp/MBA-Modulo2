import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { TransactionListModel } from '../pages/transaction/models/transaction-list.model';
import { TransactionModel } from '../pages/transaction/models/transaction.model';

@Injectable({ providedIn: 'root' })
export class TransactionService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient) {
    super();
  }

  getBalanceByMonth(date: Date): Observable<TransactionListModel[]> {
    const formattedDate = date.toISOString();
    let response = this.http
      .get(`${this.UrlServiceV1}v1/transaction/get-balance-by-month-year?date=${formattedDate}`,  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  create(transaction: TransactionModel): Observable<TransactionModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/transaction', transaction, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  update(transaction: TransactionModel): Observable<TransactionModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/transaction/' + transaction.transactionId, transaction, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

  delete(transactionId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/transaction/${transactionId}`, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

}