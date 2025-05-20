import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { Vehicle } from '../../../core/models/vehicle.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private apiVehiculosUrl = `${environment.apiBaseUrl}/api/Vehiculo`;
  private apiCatalogoUrl = `${environment.apiBaseUrl}/api/Catalogo`;

  constructor(private http: HttpClient, private router: Router) { }

  // Obtener vehiculos
  getVehiculos(): Observable<Vehicle[]> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Vehicle[]>(this.apiVehiculosUrl, { headers });
  }

  // Obtener vehiculo por id
  getVehicleById(id: number): Observable<Vehicle> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Vehicle>(`${this.apiVehiculosUrl}/${id}`, { headers });
  }

  // Crear vehiculo
  createVehicle(vehicle: any): Observable<any> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(this.apiVehiculosUrl, vehicle, { headers });
  }

  // Actualizar vehiculo
  updateVehicle(id: number, vehicleData: any): Observable<any> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<any>(`${this.apiVehiculosUrl}/${id}`, vehicleData, { headers });
  }
  
  // Borrar vehiculo
  deleteVehicle(id: number): Observable<void> {
    const token = localStorage.getItem('access_token');
    if (!token) {
      console.error('Token de autenticación no encontrado.');
      throw new Error('Usuario no autenticado');
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.apiVehiculosUrl}/${id}`, { headers });
  }

  // Obtener marcas
  getMarcas(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiCatalogoUrl}/marcas`);
  }

  // Obtener modelos
  getModelos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiCatalogoUrl}/modelos`);
  }

  createMarca(marca: any): Observable<any> {
    return this.http.post(`${this.apiCatalogoUrl}/marcas`, marca);
  }

  createModelo(modelo: any): Observable<any> {
    return this.http.post(`${this.apiCatalogoUrl}/modelos`, modelo);
  }

}
