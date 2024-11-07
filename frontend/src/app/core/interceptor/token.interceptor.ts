import { inject, Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { EMPTY, Observable, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';
import { UserAuthService } from '../services/user-auth/user-auth.service';
import { components } from '../models/models';
import { AppStore } from 'src/app/app.store';

type NewRefreshTokenDto = components['schemas']['NewRefreshTokenDto'];

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private readonly appStore = inject(AppStore);

  constructor(private userAuthService: UserAuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.appStore.$accessToken()) {
      request = this.addToken(request, this.appStore.$accessToken());
    }

    return next.handle(request).pipe(
      catchError((error) => {
        // Check if the error is due to an expired access token
        if (error.status === 401 && this.appStore.$accessToken()) {
          return this.handleTokenExpired(request, next);
        }

        return throwError(() => error);
      }),
    );
  }

  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  private handleTokenExpired(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const newRefreshTokenDto: NewRefreshTokenDto = {
      accessToken: this.appStore.$accessToken(),
      refreshToken: this.appStore.$refreshToken(),
    };

    return this.userAuthService.refreshAccessToken(newRefreshTokenDto).pipe(
      tap(token => this.appStore.updateToken(token)), // side effect to set token property on auth service
      switchMap(token => { // use transformation operator that maps to an Observable<T>
        return next.handle(this.addToken(request, token.accessToken));
      }),
      catchError((error) => {
        // Handle refresh token error (e.g., redirect to login page)
        this.appStore.logout();
        return EMPTY;
      }),
    );
  }
}
