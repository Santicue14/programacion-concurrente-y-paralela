import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { Sale } from '../../../core/models/sale.model';

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  private apiUrl = `${environment.apiBaseUrl}/api/Venta`;


  constructor(private http: HttpClient, private router: Router) { }

  // Obtener ventas
  getVentas(): Observable<Sale[]> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Sale[]>(this.apiUrl, { headers }).pipe(
      catchError(error => {
        console.error('Error fetching sales:', error);
        return throwError(() => error);
      })
    );
  }

  // Obtener venta por id
  getVentaById(id: number): Observable<Sale> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Sale>(`${this.apiUrl}/${id}`, { headers });
  }

  // Crear venta
  createVenta(venta: any): Observable<any> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(this.apiUrl, venta, { headers });
  }

  // Actualizar venta
  updateVenta(id: number, ventaData: any): Observable<any> {
    const token = localStorage.getItem('access_token');
    if (!token) { 
      this.router.navigate(['/login']);
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<any>(`${this.apiUrl}/${id}`, ventaData, { headers });
  }
  
  // Borrar venta
  deleteVenta(id: number): Observable<void> {
    const token = localStorage.getItem('access_token');
    if (!token) {
      console.error('Token de autenticación no encontrado.');
      throw new Error('Usuario no autenticado');
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }


}
