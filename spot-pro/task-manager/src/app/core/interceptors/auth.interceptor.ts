import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
	  const token = localStorage.getItem('access_token');
	  let clonedRequest = req;

	  if (token) {
		clonedRequest = req.clone({
		  setHeaders: {
			Authorization: `Bearer ${token}`,
		  },
		});
	  }

      console.log('Interceptando petición:', clonedRequest.url);

	  return next.handle(clonedRequest).pipe(
		catchError((error: HttpErrorResponse) => {
		  console.error('Error HTTP interceptado:', error.status);
		  if (error.status === 401) {
			console.warn('Error 401 detectado. Redirigiendo al login...');
			this.router.navigate(['/login']);
		  }
		  return throwError(() => error);
		})
	  );
   }


}
