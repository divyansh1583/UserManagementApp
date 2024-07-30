import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.resetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator } as AbstractControlOptions);
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
      console.log('Form submitted', this.resetForm.value);
      // Implement password reset logic here
    }
  }
}