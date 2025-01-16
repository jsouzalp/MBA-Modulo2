import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { BudgetModel } from '../pages/budget/models/budget.model';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from './message.service ';

@Injectable({ providedIn: 'root' })
export class BudgetService extends BaseService {

  constructor(private http: HttpClient, toastr: ToastrService, messageService: MessageService) {
    super(toastr, messageService);
  }

  getAll(): Observable<BudgetModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/budget', this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  create(budget: BudgetModel): Observable<BudgetModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/budget', budget, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  update(budget: BudgetModel): Observable<BudgetModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/budget/' + budget.budgetId, budget, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  delete(budgetId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/budget/${budgetId}`, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

}