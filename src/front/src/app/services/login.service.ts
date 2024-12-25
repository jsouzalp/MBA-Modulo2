import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable } from 'rxjs';
import { BaseService } from './BaseService';
import { LoginModel } from '../pages/authentication/models/login.model';
import { LoginRecoveryModel } from '../pages/authentication/models/login-recovery.model';
import { LoginResetModel } from '../pages/authentication/models/login-reset.model';

@Injectable({ providedIn: 'root' })
export class LoginService extends BaseService {

    public currentUrl = new BehaviorSubject<any>(undefined);

    constructor(private http: HttpClient) {
        super();
    }
    login(login: LoginModel): Observable<LoginModel> {
        let response = this.http
            .post(this.UrlServiceV1 + 'Auth/user-login', login, this.getHeaderJson())
            .pipe(
                map(this.extractData),
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

}