import { Injectable } from '@angular/core';
import { components } from '../../models/models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

type MetricDto = components['schemas']['MetricDto'];

const controller: string = "metrics";

@Injectable({
  providedIn: 'root'
})
export class MetricsService {

  constructor(private http: HttpClient) {}

  getDashboardMetrics(): Observable<MetricDto[]> {
      return this.http.get<MetricDto[]>(`${environment.apiUrl}/${controller}/dashboard`);
  }
}
