import { computed, Injectable, signal } from '@angular/core';
import { components } from './core/models/models';
import { createEffect } from 'src/create-effect';
import { catchError, exhaustMap, finalize, tap } from 'rxjs';
import { UserAuthService } from './core/services/user-auth/user-auth.service';
import { Router } from '@angular/router';
import { handleRequestError } from './core/utils/custom-functions';

type LoginUserDto = components['schemas']['LoginUserDto'];
type LoginUserResponseDto = components['schemas']['LoginUserResponseDto'];
type NewRefreshTokenDto = components['schemas']['NewRefreshTokenDto'];

@Injectable({ providedIn: 'root' })
export class AppStore {
  private readonly _tokenKey: string = 'ims-token';
  private readonly _stringifiedToken = localStorage.getItem(this._tokenKey);
  private readonly _storedToken: LoginUserResponseDto = this._stringifiedToken
    ? JSON.parse(this._stringifiedToken)
    : {};

  private readonly state = {
    $token: signal<LoginUserResponseDto>({
      accessToken: this._storedToken.accessToken ?? '',
      refreshToken: this._storedToken.refreshToken ?? '',
      expiresAt: this._storedToken.expiresAt ?? '',
      username: this._storedToken.username ?? '',
    }),
    $loggingIn: signal<boolean>(false),
  } as const;

  public readonly $token = this.state.$token.asReadonly();
  public readonly $loggingIn = this.state.$loggingIn.asReadonly();
  public readonly $isAuthenticated = computed<boolean>(() => this.$token().accessToken !== '');

  constructor(private userAuthService: UserAuthService, private readonly _router: Router) {}

  login(credentials: LoginUserDto) {
    this.handleLogin(credentials);
  }
  
  logout() {
    localStorage.removeItem(this._tokenKey);
    this.reset();
    this._router.navigate(['/auth/sign-in']);
  }
  
  refreshAccessToken() {
    const newRefreshTokenDto: NewRefreshTokenDto = {
      accessToken: this._storedToken.accessToken,
      refreshToken: this._storedToken.refreshToken,
    };

    this.handleRefreshAccessToken(newRefreshTokenDto);
  }

  reset() {
    this.state.$token.set({
      accessToken: '',
      refreshToken: '',
      expiresAt: '',
      username: ''
    })
  }

  private handleLogin = createEffect<LoginUserDto>((_) =>
    _.pipe(
      exhaustMap((credentials) => {
        this.state.$loggingIn.set(true);
        return this.userAuthService.login(credentials).pipe(
          finalize(() => this.state.$loggingIn.set(false)),
          tap((response) => {
            localStorage.setItem(this._tokenKey, JSON.stringify(response));
            this.state.$token.set(response);
            this._router.navigate(['/']);
          }),
          catchError(async (error) => handleRequestError(error)),
        );
      }),
    ),
  );
  
  private handleRefreshAccessToken = createEffect<NewRefreshTokenDto>((_) =>
    _.pipe(
      exhaustMap((newRefreshTokenDto) => {
        return this.userAuthService.refreshAccessToken(newRefreshTokenDto).pipe(
          tap((response) => {
            localStorage.setItem(this._tokenKey, JSON.stringify(response));
            this.state.$token.set(response);
          }),
          catchError(async (error) => {
            handleRequestError(error);
            this._router.navigate(['/auth/sign-in']);
          }),
        );
      }),
    ),
  );
}
