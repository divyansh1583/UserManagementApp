import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';
import { TokenService } from '../services/token.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private tokenService: TokenService
  ) {
    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmNewPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('newPassword')?.value === g.get('confirmNewPassword')?.value
      ? null : {'mismatch': true};
  }
  varyy={};
  onSubmit() {
    if (this.changePasswordForm.valid) {
      const userId = this.tokenService.getUserId();
      const changePasswordDto = {
        userId: userId,
        currentPassword: this.changePasswordForm.get('currentPassword')?.value,
        newPassword: this.changePasswordForm.get('newPassword')?.value
      };
      this.varyy=changePasswordDto;
      console.log(changePasswordDto);
      this.authService.changePassword(changePasswordDto).subscribe(res=>{
        if(res.statusCode==200){
          this.toastr.success(res.message);
        }else{
          this.toastr.error(res.message);
        }
      });
    }
  }

}
