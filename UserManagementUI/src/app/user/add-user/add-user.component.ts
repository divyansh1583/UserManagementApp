import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { UserDto } from '../../shared/models/user-dto';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  userForm: FormGroup = this.fb.group({});
  imagePreview: string | ArrayBuffer | null = null;
  showSecondaryAddress: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.userForm = this.fb.group({
      firstName: ['', Validators.required],
      middleName: [''],
      lastName: ['', Validators.required],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfJoining: ['', Validators.required],
      phone: ['', Validators.required],
      alternatePhone: [''],
      isActive: [true],
      addresses: this.fb.array([
        this.createAddressFormGroup(1),  // Primary address
        this.createAddressFormGroup(2)   // Secondary address
      ])
    });
  }

  createAddressFormGroup(addressTypeId: number): FormGroup {
    return this.fb.group({
      address: ['Default', Validators.required],
      cityId: [1, Validators.required],
      stateId: [1, Validators.required],
      countryId: [1, Validators.required],
      zipCode: [1, Validators.required],
      addressTypeId: [addressTypeId]
    });
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

  async onSubmit() {
    if (this.userForm.valid) {
      const formData = new FormData();
      const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
      const file = fileInput.files?.[0];

      if (file) {
        formData.append('file', file);
        try {
          const response = await this.userService.uploadImage(formData).toPromise();
          if (response && response.data) {
            const userData = this.prepareUserData(response.data);
            this.addUser(userData);
          }
        } catch (error) {
          console.log(error);
          this.toastr.error('Error uploading image!');
        }
      } else {
        this.toastr.warning('Please select an image to upload');
      }
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