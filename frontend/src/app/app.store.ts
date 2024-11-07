import { computed, Injectable, signal } from '@angular/core';
import { components } from './core/models/models';
import { createEffect } from 'src/create-effect';
import { catchError, exhaustMap, finalize, of, tap } from 'rxjs';
import { UserAuthService } from './core/services/user-auth/user-auth.service';
import { Router } from '@angular/router';
import { handleRequestError } from './core/utils/custom-functions';

type LoginUserDto = components['schemas']['LoginUserDto'];
type LoginUserResponseDto = components['schemas']['LoginUserResponseDto'];
type NewRefreshTokenDto = components['schemas']['NewRefreshTokenDto'];

@Injectable({ providedIn: 'root' })
export class AppStore {
  private readonly _tokenKey: string = 'ims-token';
  private readonly state = {
    $token: signal<LoginUserResponseDto>({
      accessToken: '',
      refreshToken: '',
      expiresAt: '',
      username: '',
    }),
    $loggingIn: signal<boolean>(false),
  } as const;

  public readonly $token = this.state.$token.asReadonly();
  public readonly $loggingIn = this.state.$loggingIn.asReadonly();
  public readonly $accessToken = computed<string>(() => this.$token().accessToken);
  public readonly $refreshToken = computed<string>(() => this.$token().refreshToken);
  public readonly $isAuthenticated = computed<boolean>(() => this.$token().accessToken !== '' && this.isLoggedIn());

  constructor(private userAuthService: UserAuthService, private readonly _router: Router) {
    this.loadState();
  }

  login(credentials: LoginUserDto) {
    this.handleLogin(credentials);
  }

  logout() {
    localStorage.removeItem(this._tokenKey);
    this.reset();
    this._router.navigate(['/auth/sign-in']);
  }

  refreshAccessToken(newRefreshTokenDto: NewRefreshTokenDto) {
    return of(this.handleRefreshAccessToken(newRefreshTokenDto));
  }

  reset() {
    this.state.$token.set({
      accessToken: '',
      refreshToken: '',
      expiresAt: '',
      username: ''
    })
  }

  updateToken(newAccessToken: LoginUserResponseDto) {
    localStorage.setItem(this._tokenKey, JSON.stringify(newAccessToken));
    this.state.$token.set(newAccessToken);
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
          catchError(async (error) => {
            handleRequestError(error);
          }),
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
            return response;
          }),
          catchError(async (error) => {
            handleRequestError(error);
          }),
        );
      }),
    ),
  );

  private loadState() {
    const stringifiedSavedState = localStorage.getItem(this._tokenKey);

    if (stringifiedSavedState) {
      const savedState: LoginUserResponseDto = JSON.parse(localStorage.getItem(this._tokenKey)!);
      this.state.$token.set(savedState);
      this.state.$loggingIn.set(false);
    }
  }

  private isLoggedIn() {
    var expiresAt = new Date(this.$token().expiresAt);
    var currentDate = new Date();

    if (currentDate > expiresAt)
      return false
    else
      return true
  }
}
