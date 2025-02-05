import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { components, paths } from '../../models/models';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

type InstallmentsQuery = paths['/api/installments']['get']['parameters']['query']
type InstallmentsDto = components['schemas']['InstallmentsDto'];
type NewInstallmentDto = components['schemas']['NewInstallmentDto'];
type UpdatedInstallmentDto = components['schemas']['UpdatedInstallmentDto'];
type InstallmentDto = components['schemas']['InstallmentDto'];

const controller: string = "installments";

@Injectable({
  providedIn: 'root'
})
export class InstallmentsService {

  constructor(private http: HttpClient) {}

  getInstallments(query?: InstallmentsQuery): Observable<InstallmentsDto> {
    return this.http.get<InstallmentsDto>(`${environment.apiUrl}/${controller}`, {params: query});
  }
  
  createInstallment(newInstallmentDto: NewInstallmentDto): Observable<InstallmentDto> {
    return this.http.post<InstallmentDto>(`${environment.apiUrl}/${controller}`, newInstallmentDto);
  }
  
  updateInstallment(id:string, updatedInstallmentDto: UpdatedInstallmentDto): Observable<InstallmentDto> {
    return this.http.put<InstallmentDto>(`${environment.apiUrl}/${controller}/${id}`, updatedInstallmentDto);
  }
  
  deleteInstallment(id:string): Observable<InstallmentDto> {
    return this.http.delete<InstallmentDto>(`${environment.apiUrl}/${controller}/${id}`);
  }
}
