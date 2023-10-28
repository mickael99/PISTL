import { Injectable } from '@angular/core';
import { AddSessionRequest } from '../models/add-session-request-model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private http: HttpClient) { }

  addSession(model: AddSessionRequest): Observable<void> {
    return this.http.post<void>('https://localhost:7186/api/session', model);
  }
}
