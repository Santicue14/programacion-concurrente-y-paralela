import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Client } from '../../../core/models/client.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private apiClienteUrl = `${environment.apiBaseUrl}/api/Cliente`;

  constructor(private http: HttpClient, private router: Router) { }

  // Obtener clientes
  getClients(): Observable<Client[]> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
      return new Observable<Client[]>();
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Client[]>(this.apiClienteUrl, { headers });
  }

  // Obtener cliente por id
  getClientById(id: number): Observable<Client> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
      return new Observable<Client>();
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Client>(`${this.apiClienteUrl}/${id}`, { headers });
  }

  // Crear cliente
  createClient(client: Client): Observable<Client> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
      return new Observable<Client>();
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<Client>(this.apiClienteUrl, client, { headers });
  }

  // Actualizar cliente
  updateClient(id: number, clientData: Client): Observable<Client> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
      return new Observable<Client>();
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<Client>(`${this.apiClienteUrl}/${id}`, clientData, { headers });
  }
  
  // Borrar cliente
  deleteClient(id: number): Observable<void> {
    const token = localStorage.getItem('access_token');
    if (!token) {
      this.router.navigate(['/login']);
      return new Observable<void>();
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.apiClienteUrl}/${id}`, { headers });
  }
} 