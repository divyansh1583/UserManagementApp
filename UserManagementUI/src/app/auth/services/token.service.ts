import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private token: string;
  private claimNames = {
    id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
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
  isTokenValid(): boolean {
    if (!this.token) return false;
    const decodedToken = JSON.parse(atob(this.token.split('.')[1]));
    const expiryTime = decodedToken.exp;
    return expiryTime > Date.now() / 1000;
  }
}
