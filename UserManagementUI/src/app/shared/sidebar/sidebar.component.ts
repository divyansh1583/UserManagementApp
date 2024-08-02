import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  menuItems = [
    { icon: 'bi-speedometer2', label: 'Dashboard', route: '/user/dashboard' },
    { icon: 'bi-person', label: 'Manage User', route: '/user/add-user' }
  ];
  @Input() isOpen = true;
  isManageUserOpen = false;

  toggleManageUser() {
    this.isManageUserOpen = !this.isManageUserOpen;
  }
}
