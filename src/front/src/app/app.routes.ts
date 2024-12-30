import { Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';

import { AuthGuard } from './auth.guard';
import { LoginComponent } from './pages/user/login/login.component';
import { ForgotPasswordComponent } from './pages/user/forgot-password/forgot-password.component';
import { RestorePasswordComponent } from './pages/user/restore-password/restore-password.component';

export const routes: Routes = [
  {
    path: '',
    component: FullComponent,
    children: [
      {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full',
      },
      {
        path: 'pages',
        loadChildren: () => import('./pages/pages.routes').then((m) => m.PagesRoutes),
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard]
      },
      {
        path: 'ui-components',
        loadChildren: () => import('./pages/ui-components/ui-components.routes').then((m) => m.UiComponentsRoutes),
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard]
      },
      {
        path: 'extra',
        loadChildren: () => import('./pages/extra/extra.routes').then((m) => m.ExtraRoutes),
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard]
      }
      ,
    ],
  },
  {
    path: '',
    component: BlankComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () => import('./pages/user/user.routes').then((m) => m.UserRoutes),
      },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: 'authentication/forgot-password', component: ForgotPasswordComponent },
  { path: 'restore-password', component: RestorePasswordComponent },
  { path: '**', component: NotFoundComponent }
];
