import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SortableHeaderDirective } from './directives/sortable-header.directive';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import {MatListModule} from '@angular/material/list';

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
    MatIconModule,
    MatListModule
  ],
  exports:[
    HeaderComponent,
    SidebarComponent
  ]
})
export class SharedModule { }
