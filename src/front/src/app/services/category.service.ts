import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { LoginRecoveryModel } from '../pages/user/models/login-recovery.model';
import { LoginResetModel } from '../pages/user/models/login-reset.model';
import { LoginModel } from '../pages/user/models/login.model';
import { UserTokenModel } from '../pages/user/models/user-token.model';
import { UserRegisterModel } from '../pages/user/models/user-register.model';
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

  register(user: UserRegisterModel): Observable<UserTokenModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/category/register', user, this.getAuthHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

}