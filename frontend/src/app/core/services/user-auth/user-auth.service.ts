import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { components } from '../../models/models';
import { handleRequestError } from '../../utils/custom-functions';

type LoginUserDto = components['schemas']['LoginUserDto'];
type LoginUserResponseDto = components['schemas']['LoginUserResponseDto'];
type UserDto = components['schemas']['UserDto'];
type NewRefreshTokenDto = components['schemas']['NewRefreshTokenDto'];

const controller: string = 'users';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private readonly _tokenKey: string = 'ims-token';
  private readonly _stringifiedToken = localStorage.getItem(this._tokenKey);
  private readonly _storedToken: LoginUserResponseDto = this._stringifiedToken
    ? JSON.parse(this._stringifiedToken)
    : {};

  constructor(private http: HttpClient) {}

  login(credentials: LoginUserDto): Observable<LoginUserResponseDto> {
    return this.http.post<LoginUserResponseDto>(`${environment.apiUrl}/${controller}/login`, credentials);
  }
  
  getCurrentUser(): Observable<UserDto> {
    return this.http.get<UserDto>(`${environment.apiUrl}/${controller}/me`);
  }

  logout() {
    localStorage.removeItem(this._tokenKey);
  }

  refreshAccessToken(newRefreshTokenDto: NewRefreshTokenDto): Observable<LoginUserResponseDto> {
    return this.http.post<LoginUserResponseDto>(`${environment.apiUrl}/${controller}/refresh`, newRefreshTokenDto);
  }
}
