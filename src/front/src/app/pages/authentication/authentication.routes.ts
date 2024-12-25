import { Routes } from '@angular/router';

import { AppSideLoginComponent } from './side-login/side-login.component';
import { UserRegisterComponent } from './register/register.component';

export const AuthenticationRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: AppSideLoginComponent,
      },
      {
        path: 'register',
        component: UserRegisterComponent,
      },
    ],
  },
];
