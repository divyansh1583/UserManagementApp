import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SortableHeaderDirective } from './directives/sortable-header.directive';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';


@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    SortableHeaderDirective
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    NgbDropdownModule,
    MatMenuModule,
    MatIconModule
  ],
  exports:[
    HeaderComponent,
    SidebarComponent
  ]
})
export class SharedModule { }
