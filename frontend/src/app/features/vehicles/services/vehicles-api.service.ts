import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

import { Router } from '@angular/router';

import { Vehicle } from '../../../core/models/Vehicle.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private apiUrl = `${environment.apiBaseUrl}/api/`;

  constructor(private http: HttpClient, private router: Router) { }

  // Obtener vehiculos
  getVehiculos(): Observable<Vehicle[]> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Vehicle[]>(this.apiUrl, { headers });
  }

  // Obtener tarea por id
  getVehicleById(id: number): Observable<Vehicle> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Vehicle>(`${this.apiUrl}/${id}`, { headers });
  }

  // Crear tarea
  createVehicle(vehicle: Vehicle): Observable<Vehicle> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<Vehicle>(this.apiUrl, vehicle, { headers });
  }

  // Actualizar tarea
  updateVehicle(id: number, vehicleData: Vehicle): Observable<Vehicle> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<Vehicle>(`${this.apiUrl}/${id}`, vehicleData, { headers });
  }
  
  // Actualizar tarea
  deleteVehicle(id: number): Observable<void> {
    const token = localStorage.getItem('access_token');
    if (!token) {
      console.error('Token de autenticación no encontrado.');
      throw new Error('Usuario no autenticado');
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }

}
