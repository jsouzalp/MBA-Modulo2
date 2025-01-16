import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { CategoryModel } from '../pages/category/models/category.model';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from './message.service ';

@Injectable({ providedIn: 'root' })
export class CategoryService extends BaseService {

  constructor(private http: HttpClient, toastr: ToastrService, messageService: MessageService) {
    super(toastr, messageService);
  }

  getAll(): Observable<CategoryModel[]> {
    let response = this.http
      .get(this.UrlServiceV1 + 'v1/category', this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  create(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/category', category, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  update(category: CategoryModel): Observable<CategoryModel> {
    let response = this.http
      .put(this.UrlServiceV1 + 'v1/category/' + category.categoryId, category, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

  delete(categoryId: string): Observable<void> {
    let response = this.http
      .delete(this.UrlServiceV1 + `v1/category/${categoryId}`, this.getAuthHeaderJson())
      .pipe(
        map(response => this.extractData(response)),
        catchError(error => this.serviceError(error)));

    return response;
  }

}