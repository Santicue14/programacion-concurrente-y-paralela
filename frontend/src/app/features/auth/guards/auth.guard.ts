import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate( next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
	const isAuth = this.authService.isAuthenticated();
	if (isAuth) {
		return true;
	} 
	else {
		console.warn('Usuario no autenticado. Redirigiendo a login.');
		this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
		return false;
	}
  } 


}