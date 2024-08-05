import { Component, EventEmitter, HostListener, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from 'src/app/auth/services/token.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  userName: string;
  imagePath: any;
  constructor(
    private tokenService: TokenService,
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

  changePassword() {
  
    this.router.navigate(['/change-password']);
  }

  logOut() {
    localStorage.removeItem('login_token');
    this.router.navigate(['/login']);
  }
}
