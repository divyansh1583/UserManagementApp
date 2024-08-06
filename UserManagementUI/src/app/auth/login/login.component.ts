import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginDetails } from 'src/app/shared/models/login.modal';
import { AuthService } from '../services/auth.service';

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
    private authService:AuthService,
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
      this.authService.login(this.loginDetails).subscribe({
        next:(res)=> {
          if (res.statusCode === 200) {
            localStorage.setItem('login_token', res.data.token);
            this.router.navigate(['/user']);
            this.toastr.success(res.message);
          }
          else {
            console.log(res);
            this.toastr.error(res.message);
          }
        },
        error: (err) => {
          this.toastr.error(err.message);
        }
      });
    }
    else{
      this.toastr.error('Invalid Credentials!')
    }
  }
}

