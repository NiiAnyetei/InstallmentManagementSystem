import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { components, paths } from '../../models/models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

type PaymentsQuery = paths['/api/payments']['get']['parameters']['query'];
type PaymentsDto = components['schemas']['PaymentsDto'];

const controller: string = 'payments';

@Injectable({
  providedIn: 'root',
})
export class PaymentsService {
  constructor(private http: HttpClient) {}

  getPayments(query?: PaymentsQuery): Observable<PaymentsDto> {
    return this.http.get<PaymentsDto>(`${environment.apiUrl}/${controller}`, { params: query });
  }
}
