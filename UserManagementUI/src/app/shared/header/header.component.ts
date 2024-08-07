import { Component, EventEmitter, HostListener, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from 'src/app/auth/services/token.service';
import { UserService } from 'src/app/user/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  userName: string="";
  imagePath: any;
  constructor(
    private tokenService: TokenService,
    private router: Router,
    private userService:UserService,
    private toastr:ToastrService
  ) {
    // this.userName = this.tokenService.getName();
    // this.imagePath = this.tokenService.getImagePath();
    this.userService.getUserById(this.tokenService.getUserId()).subscribe({
      next: (res) => {
        if(res.statusCode==200){
          console.log(res.data);
          this.userName=res.data.firstName+" "+res.data.lastName;
          this.imagePath="https://localhost:7118" +res.data.imagePath;
        }
        else{
          console.log(res.message);
        }
      },
      error:(err)=>{
        this.toastr.error(err.message);
      }
    });
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
