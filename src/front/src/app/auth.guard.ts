import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LocalStorageUtils } from './utils/localstorage';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils) { }

  canActivate(): boolean {
    if (this.isAuthenticated() && this.isValidToken()) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }

  private isAuthenticated(): boolean {
    const token = this.localStorageUtils.getUserToken();
    return !!token;
  }
  private isValidToken(): boolean {
    const now = new Date();

    var dateLogged = new Date(this.localStorageUtils.getExpiresAt());

    if (now > dateLogged) {
      return false;
    }

    return true;
  }

}


