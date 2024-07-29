import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7118'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  // Corresponds to UserController.GetUsers
  getUsers(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getAllUsers`);
  }

}
