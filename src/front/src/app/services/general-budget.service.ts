import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { GeneralBudgetModel } from '../pages/budget/models/general-budget.model';

@Injectable({ providedIn: 'root' })
export class GeneralBudgetService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient) {
    super();
  }

  getAll(): Observable<GeneralBudgetModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/generalbudget',  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  create(budget: GeneralBudgetModel): Observable<GeneralBudgetModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/generalbudget', budget, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  update(budget: GeneralBudgetModel): Observable<GeneralBudgetModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/generalbudget/' + budget.generalBudgetId , budget, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

  delete(budgetId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/generalbudget/${budgetId}`, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

}