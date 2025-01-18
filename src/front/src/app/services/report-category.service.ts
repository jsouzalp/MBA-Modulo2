import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { ReportCategoryAnalytics } from '../pages/reports/category-transaction-analytics/Models/transaction.models';
import { ReportCategory } from '../pages/reports/category-transaction-summary/models/transaction.models';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from './message.service ';




@Injectable({ providedIn: 'root' })
export class ReportCategoryService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient, toastr: ToastrService, messageService: MessageService) {
    super(toastr, messageService);
  }

  getAnalytics(startDate: Date | null , endDate: Date | null): Observable<ReportCategoryAnalytics[]> {
    
    let param1 = '';
    let param2 = '';
    
    if (startDate)
      param1 = this.formatDate(startDate);

    if (endDate)
      param2 = this.formatDate(endDate);
    
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/Report/Transactions/AnalyticsByCategory?startDate=' +  param1 + '&endDate=' + param2,  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  getSummary(startDate: Date | null , endDate: Date | null): Observable<ReportCategory[]> {

    let param1 = '';
    let param2 = '';
    
    if (startDate)
      param1 = this.formatDate(startDate);

    if (endDate)
      param2 = this.formatDate(endDate);


    let response = this.http
      .get(this.UrlServiceV1 + 'v1/Report/Transactions/SummaryByCategory?startDate=' +  param1 + '&endDate=' + param2  ,  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

 
  getPdfSummary(startDate?: Date | null , endDate?: Date | null) {

    let param1 = '';
    let param2 = '';
    
    if (startDate)
      param1 = this.formatDate(startDate);

    if (endDate)
      param2 = this.formatDate(endDate);

      return this.http.get(this.UrlServiceV1 + 'v1/Report/Transactions/SummaryByCategory/export-report?startDate=' +  param1 + '&endDate=' + param2 + '&fileType=pdf', { headers: this.getAuthHeaderOnly(), responseType: 'blob', observe: 'response' })

  }

  getXlsxSummary(startDate?: Date | null , endDate?: Date | null) {

    let param1 = '';
    let param2 = '';
    
    if (startDate)
      param1 = this.formatDate(startDate);

    if (endDate)
      param2 = this.formatDate(endDate);

      return this.http.get(this.UrlServiceV1 + 'v1/Report/Transactions/SummaryByCategory/export-report?startDate=' +  param1 + '&endDate=' + param2 + '&fileType=xlsx', { headers: this.getAuthHeaderOnly(), responseType: 'blob', observe: 'response' })

  }

}