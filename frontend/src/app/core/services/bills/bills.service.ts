import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { components, paths } from '../../models/models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

type BillsQuery = paths['/api/bills']['get']['parameters']['query'];
type BillsDto = components['schemas']['BillsDto'];

const controller: string = 'bills';

@Injectable({
  providedIn: 'root',
})
export class BillsService {

  constructor(private http: HttpClient) {}

  getBills(query?: BillsQuery): Observable<BillsDto> {
    return this.http.get<BillsDto>(`${environment.apiUrl}/${controller}`, { params: query });
  }
}
