import { Component, EventEmitter, HostListener, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SidebarService } from 'src/app/auth/services/sidebar.service';
import { TokenService } from 'src/app/auth/services/token.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  userName: string;
  imagePath: any;
  // constructor(
  //   private tokenService:TokenService,
  //   private sidebarService: SidebarService,
  //   private router: Router
  // ){
  //   this.userName = this.tokenService.getName();
  //   this.imagePath = this.tokenService.getImagePath();
  // }
  // toggleSidebar() {
  //   this.sidebarService.toggle();
  // }
  // logout() {
  //   localStorage.removeItem('login_token');
  //   this.router.navigate(['/login']);
  // }
  constructor(
    private tokenService: TokenService,
    private sidebarService: SidebarService,
    private router: Router
  ) {
    this.userName = this.tokenService.getName();
    this.imagePath = this.tokenService.getImagePath();
  }
  @Output() toggleSidebar = new EventEmitter<void>();
  isProfileDropdownOpen = false;

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }
  @HostListener('document:click')
  closeProfileDropdown() {
    this.isProfileDropdownOpen = false;
  }
  toggleProfileDropdown(event: Event) {
    event.stopPropagation();
    this.isProfileDropdownOpen = !this.isProfileDropdownOpen;
  }

  changePassword(event: Event) {
    event.preventDefault();
    this.router.navigate(['/change-password']);
  }

  logout(event: Event) {
    event.preventDefault();
    localStorage.removeItem('login_token');
    this.router.navigate(['/login']);
  }
}
