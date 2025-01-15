import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { ReportCategoryAnalytics } from '../pages/reports/category-transaction-analytics/Models/transaction.models';




@Injectable({ providedIn: 'root' })
export class ReportCategoryService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient) {
    super();
  }

  getAnalytics(): Observable<ReportCategoryAnalytics[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/Report/Transactions/AnalyticsByCategory?startDate=2025-01-01&endDate=2025-01-31',  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  getSummary(): Observable<ReportCategoryAnalytics[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/Report/Transactions/AnalyticsByCategory?startDate=2025-01-01&endDate=2025-01-31',  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }


}