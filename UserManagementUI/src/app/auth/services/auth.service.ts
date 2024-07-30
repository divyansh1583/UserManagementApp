import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDetails } from 'src/app/shared/models/login.modal';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7118/api/Users'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  // Corresponds to UserController.LoginAsync
  login(loginDetails: LoginDetails): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginDetails);
  }
}
