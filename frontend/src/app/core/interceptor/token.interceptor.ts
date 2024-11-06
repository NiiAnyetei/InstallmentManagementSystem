import { inject, Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { UserAuthService } from '../services/user-auth/user-auth.service';
import { components } from '../models/models';
import { AppStore } from 'src/app/app.store';

type LoginUserResponseDto = components['schemas']['LoginUserResponseDto'];
type NewRefreshTokenDto = components['schemas']['NewRefreshTokenDto'];

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private readonly appStore = inject(AppStore);

  private readonly token = this.appStore.$token();

  constructor(private authService: UserAuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.token.accessToken) {
      request = this.addToken(request, this.token.accessToken);
    }

    return next.handle(request).pipe(
      catchError((error) => {
        // Check if the error is due to an expired access token
        if (error.status === 401 && this.token.accessToken) {
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
      accessToken: this.token.accessToken,
      refreshToken: this.token.refreshToken,
    };

    // Call the refresh token endpoint to get a new access token
    return this.authService.refreshAccessToken(newRefreshTokenDto).pipe(
      switchMap(() => {
        const newAccessToken = this.token.accessToken;
        // Retry the original request with the new access token
        return next.handle(this.addToken(request, newAccessToken));
      }),
      catchError((error) => {
        // Handle refresh token error (e.g., redirect to login page)
        console.error('Error handling expired access token:', error);
        return throwError(() => error);
      }),
    );
  }
}
