import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddUserComponent } from './add-user/add-user.component';
import { UserComponent } from './user.component';
import { SharedModule } from '../shared/shared.module';
import { NgbDropdownModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import {MatCheckboxModule} from '@angular/material/checkbox';

@NgModule({
  declarations: [
    DashboardComponent,
    AddUserComponent,
    UserComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,
    NgbPaginationModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxMaskDirective,
    NgbDropdownModule,
    FormsModule,
    MatCheckboxModule
  ],
  providers:[
    provideNgxMask()
  ]
})
export class UserModule { }
