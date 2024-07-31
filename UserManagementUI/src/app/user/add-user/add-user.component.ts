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

  userForm: FormGroup=this.fb.group({});
  imagePreview: string | ArrayBuffer | null = null;
  showSecondaryAddress: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit(): void {
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
      imagePath:[null],
      password:["12345@Dc"],
      isActive: [true],
      addresses: this.fb.array([
        this.createAddressFormGroup(1),  // Primary address
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

  get addressesFormArray(): FormArray {
    return this.userForm.get('addresses') as FormArray;
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
    if (this.showSecondaryAddress) {
      this.addressesFormArray.push(this.createAddressFormGroup(2));
    } else {
      this.addressesFormArray.removeAt(1);
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      console.log(this.userForm.value);
      const formData = new FormData();
      const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
      const file = fileInput.files?.[0];

      if (file) {
        formData.append('file', file);
        console.log(formData);
        try {
          this.userService.uploadImage(formData).subscribe(response => {
            console.log(response);
            if (response && response.data) {
              const userData = this.prepareUserData(response.data);
              console.log(userData);
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
    } else {
      this.toastr.warning('Please fill all the required fields');
    }
  }

  prepareUserData(imagePath: string) {
    this.userForm.get('imagePath')!.setValue(imagePath);
    const formValue = this.userForm.value;
    const userData: UserDto = {
      ...formValue
    };
    return userData;
  }

  addUser(userDto: UserDto) {
    this.userService.addUser(userDto).subscribe(res => {
      if (res.statusCode == 200||res.statusCode == 201) {
        console.log(res);
        this.router.navigate(['/user/dashboard']);
        this.toastr.success(res.message);
      } else {
        console.log(res);
        this.toastr.error(res.message);
      }
    });
  }
}