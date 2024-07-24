import { CanActivateFn } from '@angular/router';

export const UserGuard: CanActivateFn = (route, state) => {
  if (localStorage.getItem('login_token')) {
    return true; 
  } else {
    return false; 
  }
};
