import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { CategoryModel } from '../pages/category/models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient) {
    super();
  }

  getAll(): Observable<CategoryModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/category/get-all',  this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  create(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/category/create', category, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  update(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/category/update', category, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

  delete(categoryId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/category/delete/${categoryId}`, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }  

}