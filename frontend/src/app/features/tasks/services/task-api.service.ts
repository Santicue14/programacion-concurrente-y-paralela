import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

import { Router } from '@angular/router';

import { Task } from '../../../core/models/task.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = `${environment.apiBaseUrl}/tasks`;

  constructor(private http: HttpClient, private router: Router) { }

  // Obtener tareas
  getTasks(): Observable<Task[]> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Task[]>(this.apiUrl, { headers });
  }

  // Obtener tarea por id
  getTaskById(id: number): Observable<Task> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Task>(`${this.apiUrl}/${id}`, { headers });
  }

  // Crear tarea
  createTask(task: Task): Observable<Task> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<Task>(this.apiUrl, task, { headers });
  }

  // Actualizar tarea
  updateTask(id: number, taskData: Task): Observable<Task> {
    const token = localStorage.getItem('access_token');
	if (!token) { 
		this.router.navigate(['/login']);
	}
	const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<Task>(`${this.apiUrl}/${id}`, taskData, { headers });
  }
  
  // Actualizar tarea
  deleteTask(id: number): Observable<void> {
    const token = localStorage.getItem('access_token');
    if (!token) {
      console.error('Token de autenticación no encontrado.');
      throw new Error('Usuario no autenticado');
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }

}
