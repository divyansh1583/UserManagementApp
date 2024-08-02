import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { userGuard } from './guards/user.guard';

const routes: Routes = [
  { path: '', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
  { path: 'user', loadChildren: () => import('./user/user.module').then(m => m.UserModule), canActivate: [userGuard] },
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
