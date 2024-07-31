import { CanActivateFn } from '@angular/router';

export const userGuard: CanActivateFn = (route, state) => {
  if (localStorage.getItem('login_token')) {
    return true; 
  } else {
    return false; 
  }
};
