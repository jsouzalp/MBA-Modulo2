import { Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { UserRegisterComponent } from './register/register.component';

export const UserRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'register',
        component: UserRegisterComponent,
      },
    ],
  },
];
