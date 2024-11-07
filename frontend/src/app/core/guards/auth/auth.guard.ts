import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateChildFn, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AppStore } from 'src/app/app.store';

export const canActivate: CanActivateChildFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const appStore = inject(AppStore);

  // return appStore.$isAuthenticated() ? true : appStore.logout();
  
  if (appStore.$isAuthenticated()) {
    return true;
  } else {
    appStore.logout();
    return false;
  }
};

export const canActivateChild: CanActivateChildFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => canActivate(route, state);