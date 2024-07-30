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
      { path: 'dashboard', component: DashboardComponent },
      { path: 'add-user', component: AddUserComponent },
    ]
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
