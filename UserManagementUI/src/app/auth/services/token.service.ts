import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private token: string | null = null;
  private claimNames = {
    id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    name: 'name', // added Name claim
    imagePath: 'imagePath' // added ImagePath claim
  };
  constructor() {
    this.updateToken();
  }
  updateToken() {
    this.token = localStorage.getItem('login_token');
  }
  getExpiryTime(): number {
    if (!this.token) return 0;
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    return decodedToken.exp;
  }
  getUserIdToActivate(token: string) {
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    return decodedToken[this.claimNames.id];
  }
  getUserId(): number {
    if (!this.token) return 0;
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
    var path = "https://localhost:7118" + decodedToken[this.claimNames.imagePath];
    return path;
  }
  isTokenValid(): boolean {
    this.updateToken();
    if (!this.token) return false;
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    const expiryTime = decodedToken.exp;
    var timeNow = Date.now() / 1000;
    var isValid = expiryTime > timeNow;
    return isValid;
  }
}
