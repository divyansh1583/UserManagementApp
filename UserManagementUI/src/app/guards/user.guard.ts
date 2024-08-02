import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../auth/services/token.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const userGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const toastrService = inject(ToastrService);
  const router = inject(Router);
  if(localStorage.getItem('login_token')){
    return true;
  }
  return false;
  // if (tokenService.isTokenValid()) {
  //   return true; 
  // } else {
  //   toastrService.error('Session Ended! Please login again.', 'Error');
  //   router.navigate(['/login']); 
  //   return false; 
  // }
};
