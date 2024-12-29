import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { LoginRecoveryModel } from '../pages/user/models/login-recovery.model';
import { LoginResetModel } from '../pages/user/models/login-reset.model';
import { LoginModel } from '../pages/user/models/login.model';
import { UserTokenModel } from '../pages/user/models/user-token.model';
import { UserRegisterModel } from '../pages/user/models/user-register.model';


@Injectable({ providedIn: 'root' })
export class UserService extends BaseService {

  public currentUrl = new BehaviorSubject<any>(undefined);

  constructor(private http: HttpClient) {
    super();
  }

  login(login: LoginModel): Observable<UserTokenModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/user/login', login, this.getHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  logout(): Observable<void> {
    let response = this.http
      .post<void>(this.UrlServiceV1 + 'v1/user/logout', null, this.getAuthHeaderJson())
      .pipe(
        catchError(this.serviceError));

    return response;
  }  

  passwordRecovery(login: LoginRecoveryModel): Observable<boolean> {
    let response = this.http
      .post(this.UrlServiceV1 + `Auth/forgot-password`, login, this.getHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  restorePassword(loginReset: LoginResetModel): Observable<boolean> {
    let response = this.http
      .post(this.UrlServiceV1 + `Auth/restore-password`, loginReset, this.getHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

  register(user: UserRegisterModel): Observable<UserTokenModel> {
    let response = this.http
      .post(this.UrlServiceV1 + 'v1/user/register', user, this.getHeaderJson())
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));

    return response;
  }

}