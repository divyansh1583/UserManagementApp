import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UpdateUserDto, UserDto } from 'src/app/shared/models/user-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7118/api/Users'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  uploadImage(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/upload-image`, formData);
  }

  addUser(userDto: UserDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, userDto);
  }
  updateUser(updateUserDto: UpdateUserDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, updateUserDto);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete?id=${id}`);
  }

  getAllUsers(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getall`);
  }

  getUserById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getById?id=${id}`);
  }
}
