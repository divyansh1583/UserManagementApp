import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginDetails } from 'src/app/models/login.modal';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  hide: any;
  passwordVisible: any;
  loginDetails: LoginDetails = { email: '', password: '' };

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  onSubmit() {
    this.loginDetails.email = this.loginForm.value.email!;
    this.loginDetails.password = this.loginForm.value.password!;

    if (this.loginForm.valid) {
      this.userService.login(this.loginDetails).subscribe(res => {

        if (res.statusCode === 200) {
          console.log(res);
          localStorage.setItem('login_token', res.data.token);
          this.router.navigate(['/user']);
          this.toastr.success(res.message);
        }
        else {
          console.log(res);
          this.toastr.error(res.message);
        }
      });
    }
    else{
      this.toastr.error('Invalid Credentials!')
    }
  }
}

