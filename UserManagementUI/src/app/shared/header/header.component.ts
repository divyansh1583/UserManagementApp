import { Component } from '@angular/core';
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
    private tokenService:TokenService
  ){
    this.userName = this.tokenService.getName();
    this.imagePath = this.tokenService.getImagePath();
  }
}
