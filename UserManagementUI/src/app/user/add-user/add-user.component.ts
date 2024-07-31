import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { UserDto } from '../../shared/models/user-dto';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { cities, countries, states } from 'src/app/shared/data/location-data';

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

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit():void {
    this.initForm();
  }
  initForm() {
    this.userForm = this.fb.group({
      firstName: [null, Validators.required],
      middleName: [null],
      lastName: [null, Validators.required],
      gender: [null, Validators.required],
      dateOfBirth: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      dateOfJoining: [null, Validators.required],
      phone: [null, Validators.required],
      alternatePhone: [null],
      isActive: [true],
      addresses: this.fb.array([
        this.createAddressFormGroup(1),  // Primary address
        this.createAddressFormGroup(2)   // Secondary address
      ])
    });
  }
  createAddressFormGroup(addressTypeId: number): FormGroup {
    return this.fb.group({
      address: [null, Validators.required],
      cityId: [null, Validators.required],
      stateId: [null, Validators.required],
      countryId: [null, Validators.required],
      zipCode: [null, Validators.required],
      addressTypeId: [addressTypeId]
    });
  }
  getStates(addressIndex: number) {
  const country = this.userForm.get(`addresses.${addressIndex}.countryId`)?.value;
  if (country) {
    return this.states[country];
  }
  return [];
}

getCities(addressIndex: number) {
  const state = this.userForm.get(`addresses.${addressIndex}.stateId`)?.value;
  if (state) {
    return this.cities[state];
  }
  return [];
}

  get addressesFormArray(): FormArray {
    return this.userForm.get('addresses') as FormArray;
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
  }

  onSubmit() {
    if (this.userForm.valid) {
      console.log(this.userForm.value);
      const formData = new FormData();
      const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
      const file = fileInput.files?.[0];

      if (file) {
        formData.append('file', file);
        try {
          this.userService.uploadImage(formData).subscribe(response => {
            if (response && response.data) {
              const userData = this.prepareUserData(response.data);
              this.addUser(userData);
            }

          });
        } catch (error) {
          console.log(error);
          this.toastr.error('Error uploading image!');
        }
      } else {
        this.toastr.warning('Please select an image to upload');
      }
    }
    else{
      this.toastr.warning('Please fill all the fields');
    }
  }

  prepareUserData(imagePath: string) {
    const formValue = this.userForm.value;
    const userData: UserDto = {
      ...formValue,
      imagePath,
      addresses: formValue.addresses.filter((address: any, index: number) =>
        index === 0 || (index === 1 && this.showSecondaryAddress)
      )
    };
    return userData;
  }

  addUser(userDto: UserDto) {
    this.userService.addUser(userDto).subscribe(res => {
      if (res.statusCode == 200) {
        console.log(res);
        this.router.navigate(['/user/dashboard']);
        this.toastr.success(res.message);
      }
      else {
        console.log(res);
        this.toastr.error(res.message);
      }
    });
  }
}