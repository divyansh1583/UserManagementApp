import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private token: string;
  private claimNames = {
    id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    name: 'Name', // added Name claim
    imagePath: 'ImagePath' // added ImagePath claim
  };
  constructor() {
    this.token = localStorage.getItem('login_token')!;
  }

  getExpiryTime(): number {
    if (!this.token) return 0;
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    return decodedToken.exp;
  }

  getUserId(): string {
    if (!this.token) return '';
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    return decodedToken[this.claimNames.id];
  }

  getEmail(): string {
    if (!this.token) return '';
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    return decodedToken[this.claimNames.email];
  }
  getName(): string {
    if (!this.token) return '';
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    return decodedToken[this.claimNames.name];
  }

  getImagePath(): string {
    if (!this.token) return '';
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    var path="https://localhost:7118"+decodedToken[this.claimNames.imagePath];
    return path;
  }
  isTokenValid(): boolean {
    if (!this.token) return false;
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    const expiryTime = decodedToken.exp;
    var timeNow=Date.now() / 1000;
    var isValid=expiryTime > timeNow;
    return isValid;
  }
}
