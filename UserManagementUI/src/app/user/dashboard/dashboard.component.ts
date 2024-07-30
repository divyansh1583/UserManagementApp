import { Component } from '@angular/core';
interface User {
  firstName: string;
  middleName: string;
  lastName: string;
  dob: string;
  email: string;
  contactNo: string;
  city: string;
  state: string;
}
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  activeUsers: number = 20;
  inactiveUsers: number = 300;

  userData : any[] = [];
  paginateData: any[] = [];
  page = 1;
  pageSize = 4;
  collectionSize = 0;
  constructor(){
    this.getUserData();
    this.collectionSize = this.userData.length;
    this.getPaginatedData();
  }
  getUserData(){
    this.userData = Array(10).fill({
      firstName: 'John',
      middleName: 'Michael',
      lastName: 'Smith',
      dob: '01/05/2000',
      email: 'john@gmail.com',
      contactNo: '(503) 555-0123',
      city: 'Los Angeles',
      state: 'California'
    });
  }
  getPaginatedData(){    
    this.paginateData =  this.userData
     .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize); 
   }
}
