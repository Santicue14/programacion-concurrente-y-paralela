import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = `${environment.apiBaseUrl}`;  // Cambia según tu configuración

  constructor(private http: HttpClient, private router: Router) { }

  // Método para hacer login
  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password });
  }
  
  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }


  // Método para hacer logout
  logout_obsolete(): void {
    localStorage.removeItem('access_token');
	this.router.navigate(['/login']);
  }
  
  // Método para hacer logout
  logout(): Observable<any> {
    const token = localStorage.getItem('access_token');
    const headers = {
      'Authorization': `Bearer ${token}`
    };
    localStorage.removeItem('access_token');
    return this.http.post(`${this.apiUrl}/logout`, {}, { headers });
  }

  // Método para obtener el token de localStorage
  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  // Método para verificar si el usuario está autenticado
  isAuthenticated(): boolean {
	const token = localStorage.getItem('access_token');
	if (!token) {
		return false;
	}

	// Opcional: Decodifica el token (si es JWT) para verificar su expiración.
	try {
		const payload = JSON.parse(atob(token.split('.')[1])); // Decodifica el payload del JWT
		const now = Math.floor(new Date().getTime() / 1000); // Tiempo actual en segundos
		return payload.exp > now; // Verifica que el token no esté expirado
	} 
	catch (e) {
		console.error('Token inválido:', e);
		return false;
	}
  }
}
