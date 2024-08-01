import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserComponent } from './user.component';
import { AddUserComponent } from './add-user/add-user.component';

const routes: Routes = [
  {
    path: '', 
    component: UserComponent,
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      { path: 'dashboard', component: DashboardComponent },
      { path: 'add-user', component: AddUserComponent },
      { path: 'edit/:id', component: AddUserComponent },
    ]
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
