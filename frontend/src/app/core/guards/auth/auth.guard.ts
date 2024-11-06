import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AppStore } from 'src/app/app.store';

export const AuthGuard: CanActivateFn = (route, state) => {
  return inject(AppStore).$isAuthenticated() ? true : inject(Router).createUrlTree(['/auth/sign-in']);
};
