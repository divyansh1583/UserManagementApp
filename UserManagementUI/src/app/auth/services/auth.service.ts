import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDetails } from 'src/app/shared/models/login.modal';
import { ResetPasswordDto } from '../../shared/models/reset-password.modal';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7118/api/Auth'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  // Existing login method
  login(loginDetails: LoginDetails): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginDetails);
  }
  activateAccount(email: string, token: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/activate`, null, { params: { email, token } });
  }
  sendResetPasswordEmail(email: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/send-reset-email/${email}`, {});
  }
  // New reset password method
  resetPassword(resetPasswordDto: ResetPasswordDto): Observable<any> {
    console.log(resetPasswordDto);
    return this.http.post<any>(`${this.apiUrl}/reset-password`, resetPasswordDto);
  }
  //change password
  changePassword(changePasswordDto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/change-password`, changePasswordDto);
  }
}