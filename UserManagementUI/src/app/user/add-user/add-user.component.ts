import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { UserDto, UpdateUserDto } from '../../shared/models/user-dto';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { cities, countries, states } from 'src/app/shared/data/location-data';
import { TokenService } from 'src/app/auth/services/token.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  countries = countries;
  states: { [key: string]: { id: string; name: string; }[] } = states;
  cities: { [key: string]: { id: string; name: string; }[] } = cities;

  userForm: FormGroup = this.fb.group({});
  imagePreview: string | ArrayBuffer | null = null;
  showSecondaryAddress: boolean = false;
  isUpdateMode: boolean = false;
  userId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        this.userId = params['id'] ? +params['id'] : null;
        this.isUpdateMode = !!this.userId;
        this.initForm();
        if (this.isUpdateMode) {
          this.loadUserData();
        }
      },
      error: (err) => {
        console.error('Error in route params:', err);
      }
    });
  }

  initForm() {
    this.userForm = this.fb.group({
      firstName: [null, Validators.required],
      middleName: [null],
      lastName: [null, Validators.required],
      gender: [null, Validators.required],
      dateOfBirth: [null, Validators.required],
      dateOfJoining: [null, Validators.required],
      phone: [null, Validators.required],
      alternatePhone: [null],
      imagePath: [null],
      isActive: [true],
      addresses: this.fb.array([
        this.createAddressFormGroup(1),
      ])
    });

    if (!this.isUpdateMode) {
      this.userForm.addControl('email', this.fb.control(null, [Validators.required, Validators.email]));
      this.userForm.addControl('password', this.fb.control('12345@Dc'));
      this.userForm.addControl('createdBy', this.fb.control(this.tokenService.getEmail()));
    }
  }

  createAddressFormGroup(addressTypeId: number, address?: any): FormGroup {
    return this.fb.group({
      address: [address ? address.address : null, Validators.required],
      cityId: [address ? address.cityId : null, Validators.required],
      stateId: [address ? address.stateId : null, Validators.required],
      countryId: [address ? address.countryId : null, Validators.required],
      zipCode: [address ? address.zipCode : null, Validators.required],
      addressTypeId: [addressTypeId]
    });
  }

  get addressesFormArray(): FormArray {
    return this.userForm.get('addresses') as FormArray;
  }

  loadUserData() {
    if (this.userId) {
      this.userService.getUserById(this.userId).subscribe({
        next: (response: any) => {
          if (response.statusCode === 200 && response.data) {
            const user = response.data;
            this.userForm.patchValue({
              ...user,
              dateOfBirth: this.formatDate(user.dateOfBirth),
              dateOfJoining: this.formatDate(user.dateOfJoining),
              gender: user.gender.toLowerCase() // Normalize the gender value
            });
            this.imagePreview = user.imagePath;
            // Handle addresses
            this.addressesFormArray.clear();
            user.addresses.forEach((address: any) => {
              this.addressesFormArray.push(this.createAddressFormGroup(address.addressTypeId, address));
            });
            this.showSecondaryAddress = this.addressesFormArray.length > 1;
          } else {
            this.toastr.error('Error loading user data');
          }
        },
        error: (error) => {
          this.toastr.error('Error loading user data');
          console.error('Error loading user data:', error);
        }
      });
    }
  }

  formatDate(date: string): string {
    return new Date(date).toISOString().split('T')[0];
  }

  onSubmit() {
    if (this.userForm.valid) {
      if (this.isUpdateMode) {
        this.updateUser();
      } else {
        this.addUser();
      }
    } else {
      this.toastr.warning('Please fill all the required fields');
    }
  }

  addUser() {
    const formData = new FormData();
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    const file = fileInput.files?.[0];

    if (file) {
      formData.append('file', file);
      this.userService.uploadImage(formData).subscribe({
        next: (response) => {
          if (response.statusCode === 200 && response.data) {
            const userData = this.prepareUserData(response.data);
            this.userService.addUser(userData).subscribe({
              next: (res) => {
                if (res.statusCode === 200 || res.statusCode === 201) {
                  this.toastr.success(res.message);
                  this.router.navigate(['/user/dashboard']);
                } else {
                  this.toastr.error(res.message);
                }
              },
              error: (error) => {
                this.toastr.error('Error adding user');
                console.error('Error adding user:', error);
              }
            });
          }
        },
        error: (error) => {
          this.toastr.error('Error uploading image');
          console.error('Error uploading image:', error);
        }
      });
    } else {
      this.toastr.warning('Please select an image to upload');
    }
  }

  updateUser() {
    const updateUserDto: UpdateUserDto = {
      userId: this.userId!,
      ...this.userForm.value
    };
    this.userService.updateUser(updateUserDto).subscribe({
      next: (res) => {
        if (res.statusCode === 200) {
          this.toastr.success(res.message);
          this.router.navigate(['/user/dashboard']);
        } else {
          this.toastr.error(res.message);
        }
      },
      error: (error) => {
        this.toastr.error('Error updating user');
        console.error('Error updating user:', error);
      }
    });
  }

  prepareUserData(imagePath: string) {
    this.userForm.get('imagePath')!.setValue(imagePath);
    const formValue = this.userForm.value;
    const userData: UserDto = {
      ...formValue
    };
    return userData;
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  toggleSecondaryAddress(event: Event) {
    this.showSecondaryAddress = (event.target as HTMLInputElement).checked;
    if (this.showSecondaryAddress && this.addressesFormArray.length === 1) {
      this.addressesFormArray.push(this.createAddressFormGroup(2));
    } else if (!this.showSecondaryAddress && this.addressesFormArray.length > 1) {
      this.addressesFormArray.removeAt(1);
    }
  }

  getLocationData(field: string, index: number) {
    switch (field) {
      case 'country':
        return this.countries;
      case 'state':
        const countryId = this.userForm.get(`addresses.${index}.countryId`)?.value;
        return countryId ? this.states[countryId] : [];
      case 'city':
        const stateId = this.userForm.get(`addresses.${index}.stateId`)?.value;
        return stateId ? this.cities[stateId] : [];
      default:
        return [];
    }
  }
}