import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service'; // Import the UserService
import { ExcelDownloadService } from '../services/excel-download.service';
import { UserDto } from 'src/app/shared/models/user-dto'; // Import DTOs as needed
import { ToastrService } from 'ngx-toastr';
import { cities, states } from 'src/app/shared/data/location-data';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild('deleteDialog') deleteDialog!: TemplateRef<any>;
  activeUsers: number = 0;
  inactiveUsers: number = 0;

  userData: UserDto[] = [];
  paginateData: UserDto[] = [];
  sortedData: UserDto[] = [];
  page = 1;
  pageSize = 5;
  collectionSize = 0;
  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  // Data for lookups
  states: { [key: string]: { id: string; name: string; }[] } = states;
  cities: { [key: string]: { id: string; name: string; }[] } = cities;

  constructor(
    private userService: UserService,
    private excelDownloadService: ExcelDownloadService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: NgbModal,
  ) { }

  ngOnInit() {
    this.getUserData();
  }

  getUserData() {
    this.userService.getAllUsers().subscribe(
      res => {
        if (res.statusCode === 200) {
          this.userData = res.data;
          // Sort users by creation date, most recent first
          this.userData.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
          
          this.collectionSize = this.userData.length;
          this.activeUsers = this.userData.filter(user => user.isActive).length;
          this.inactiveUsers = this.userData.filter(user => !user.isActive).length;
          this.getPaginatedData();
        } else {
          this.toastr.error(res.message);
        }
      }
    );
  }

  getPaginatedData() {
    this.paginateData = this.sortedData.length ? this.sortedData : this.userData;
    this.paginateData = this.paginateData
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }

  downloadExcel() {
    const columnsToExport = [
      { header: 'First Name', key: 'firstName' },
      { header: 'Middle Name', key: 'middleName' },
      { header: 'Last Name', key: 'lastName' },
      { header: 'DOB', key: 'dateOfBirth' },
      { header: 'Email', key: 'email' },
      { header: 'Contact No', key: 'phone' },
      { header: 'State', key: 'state' },
      { header: 'City', key: 'city' }
    ];
  
    const dataToExport = this.paginateData.map(user => ({
      ...user,
      state: user.addresses && user.addresses.length > 0 ? this.getStateName(user.addresses[0].stateId) : 'N/A',
      city: user.addresses && user.addresses.length > 0 ? this.getCityName(user.addresses[0].cityId) : 'N/A'
    }));
  
    this.excelDownloadService.downloadExcel(dataToExport, 'users', columnsToExport);
  }

  editUser(userId: number) {
    this.router.navigate(['/user/edit', userId]);
  }

  deleteUser(userId: number) {
    const modalRef = this.modalService.open(this.deleteDialog!);
    modalRef.result.then(
      result => {
        if (result) {
          this.userService.deleteUser(userId).subscribe(
            {
              next: (res) => {
                if (res.statusCode === 200) {
                  this.getUserData();
                  this.toastr.success(res.message);
                } else {
                  this.toastr.error(res.message);
                }
              },
              error:(err)=>{
                this.toastr.error(err.message);
              }
            }
          );
        }
      }
    );
  }

  sort(column: string) {
    this.sortDirection = this.sortColumn === column ? (this.sortDirection === 'asc' ? 'desc' : 'asc') : 'asc';
    this.sortColumn = column;

    this.sortedData = this.userData.slice().sort((a, b) => {
      const isAsc = this.sortDirection === 'asc';
      switch (column) {
        case 'firstName': return compare(a.firstName, b.firstName, isAsc);
        case 'middleName': return compare(a.middleName || '', b.middleName || '', isAsc);
        case 'lastName': return compare(a.lastName, b.lastName, isAsc);
        case 'dateOfBirth': return compare(a.dateOfBirth, b.dateOfBirth, isAsc);
        case 'email': return compare(a.email, b.email, isAsc);
        case 'phone': return compare(a.phone, b.phone, isAsc);
        default: return 0;
      }
    });

    this.getPaginatedData();
  }

  getStateName(stateId: number): string {
    const state = Object.values(this.states).flat().find(state => state.id === stateId.toString());
    return state ? state.name : 'Unknown State';
  }

  getCityName(cityId: number): string {
    const city = Object.values(this.cities).flat().find(city => city.id === cityId.toString());
    return city ? city.name : 'Unknown City';
  }

}

function compare(a: string | number | Date, b: string | number | Date, isAsc: boolean): number {
  if (a < b) return isAsc ? -1 : 1;
  if (a > b) return isAsc ? 1 : -1;
  return 0;
}
