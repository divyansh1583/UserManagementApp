import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth/services/auth.service';
import { inject } from '@angular/core';
import { of, map, catchError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const resetPasswordGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const email = route.queryParams['email'];
  const token = route.queryParams['code'];

  if (!email || !token) {
    router.navigate(['/login']);
    toastr.warning("Invalid Link");
    return false;
  }
  return true;

};
