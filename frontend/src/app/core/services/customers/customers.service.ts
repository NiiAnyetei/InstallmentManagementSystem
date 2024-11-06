import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { components, paths } from '../../models/models';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

type CustomersQuery = paths['/api/customers']['get']['parameters']['query']
type CustomersDto = components['schemas']['CustomersDto'];

const controller: string = "customers";

@Injectable({
  providedIn: 'root'
})
export class CustomersService {

  constructor(private http: HttpClient) {}

  getCustomers(query?: CustomersQuery): Observable<CustomersDto> {
    return this.http.get<CustomersDto>(`${environment.apiUrl}/${controller}`, {params: query});
  }
}
