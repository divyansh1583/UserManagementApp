import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ResetPasswordDto } from 'src/app/shared/models/reset-password.modal';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup = new FormGroup({});
  email: string = '';
  emailToken: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator } as AbstractControlOptions);

    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.emailToken = decodeURIComponent(params['code']);
    });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')!;
    const confirmPassword = form.get('confirmPassword')!;
    if (password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit() {
    if (this.resetForm.valid) {
      this.isLoading = true;
      const resetPasswordDto: ResetPasswordDto = {
        Email: this.email,
        EmailToken: this.emailToken,
        NewPassword: this.resetForm.get('password')!.value
      };
      console.log(resetPasswordDto);
      this.authService.resetPassword(resetPasswordDto).subscribe({
        next: (res) => {
          if (res.statusCode === 200) {
            this.toastr.success(res.message);
            this.router.navigate(['/login']);
          }
          else {
            this.toastr.error(res.message);
          }
        },
        error: (error) => {
          this.toastr.error(error.error.message || 'An error occurred');
          if (error.error.statusCode === 400) {
            this.router.navigate(['/login']);
          }
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }
}