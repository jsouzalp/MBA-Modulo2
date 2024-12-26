import { Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { AppSideLoginComponent } from './pages/authentication/side-login/side-login.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ForgotPasswordComponent } from './pages/authentication/forgot-password/forgot-password.component';
import { RestorePasswordComponent } from './pages/authentication/restore-password/restore-password.component';
import { AuthGuard } from './auth.guard';

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
        loadChildren: () => import('./pages/authentication/authentication.routes').then((m) => m.AuthenticationRoutes),
      },
    ],
  },
  { path: 'login', component: AppSideLoginComponent },
  { path: 'authentication/forgot-password', component: ForgotPasswordComponent },
  { path: 'restore-password', component: RestorePasswordComponent },
  { path: '**', component: NotFoundComponent }
];
