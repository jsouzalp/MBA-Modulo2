import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { GeneralBudgetModel } from '../pages/budget/models/general-budget.model';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from './message.service ';

@Injectable({ providedIn: 'root' })
export class GeneralBudgetService extends BaseService {

  constructor(private http: HttpClient, toastr: ToastrService, messageService: MessageService) {
    super(toastr, messageService);
  }

  getAll(): Observable<GeneralBudgetModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/generalbudget', this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  create(budget: GeneralBudgetModel): Observable<GeneralBudgetModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/generalbudget', budget, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  update(budget: GeneralBudgetModel): Observable<GeneralBudgetModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/generalbudget/' + budget.generalBudgetId, budget, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  delete(budgetId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/generalbudget/${budgetId}`, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  exists(): Observable<any> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/generalbudget/exists', this.getAuthHeaderJson());

    return response;
  }

}