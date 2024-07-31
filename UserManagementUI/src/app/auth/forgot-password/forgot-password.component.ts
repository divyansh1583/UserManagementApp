import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  isEmailSent = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get email() {
    return this.forgotPasswordForm.get('email');
  }

  onSubmit() {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      const email = this.forgotPasswordForm.get('email')!.value;

      this.authService.sendResetPasswordEmail(email).subscribe({
        next: (res) => {
          if (res.statusCode === 200) {
            this.toastr.success(res.message);
            this.isEmailSent = true;
          }
          else {
            this.toastr.error(res.message);
            this.isEmailSent = false;
          }
        },
        error: (error) => {
          this.toastr.error(error.error.message || 'An error occurred');
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }
}
