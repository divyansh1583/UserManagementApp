import { Component, Input, SimpleChanges } from '@angular/core';

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

  isExpanded = true;
  showSubmenu: boolean = false;
  isShowing = false;


  ngOnChanges(changes: SimpleChanges) {
    if (changes['isOpen'] && !changes['isOpen'].currentValue) {
      this.isManageUserOpen = false;
    }
  }

  toggleManageUser(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    this.isManageUserOpen = !this.isManageUserOpen;
  }
}
