import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { resetPasswordGuard } from '../guards/reset-password.guard';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ActivateAccountComponent } from './login/activate.component';
import { AuthComponent } from './auth.component';

const routes: Routes = [
  
  { path: '', 
    component: AuthComponent,
    children:[
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'reset-password', component: ResetPasswordComponent, canActivate: [resetPasswordGuard] },
      
      { path: 'activate', component: ActivateAccountComponent }
    ] 
  },
  { path: 'change-password', component: ChangePasswordComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
