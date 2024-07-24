import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGuard } from './guards/user.guard';

const routes: Routes = [
  {path:'', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)},
  {path:'user', loadChildren: () => import('./user/user.module').then(m => m.UserModule),canActivate:[UserGuard]}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
