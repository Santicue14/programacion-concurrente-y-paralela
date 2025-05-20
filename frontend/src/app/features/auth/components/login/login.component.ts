import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

interface LoginError {
  success: boolean;
  token: string | null;
  message: string;
  requiereTwoFactor: boolean;
}

@Component({
  selector: 'app-login',
  imports:[FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  notificationMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService, 
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // Verificar si hay un token en la URL
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      if (token) {
        this.verifyEmail(token);
      }
    });
  }

  verifyEmail(token: string) {
    this.isLoading = true;
    this.authService.verifyEmail(token).subscribe({
      next: (response) => {
        if (response.message) {
          // Si la verificación es exitosa, mostrar mensaje y redirigir
          this.notificationMessage = response.message;
          setTimeout(() => {
            this.router.navigate(['/tasks']);
          }, 2000); // Redirigir después de 2 segundos para que el usuario vea el mensaje
        }
      },
      error: (err) => {
        console.error('Error al verificar el token:', err);
        this.errorMessage = err.error?.message || 'Error al verificar el token';
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onLogin(): void {
    this.isLoading = true;
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        console.log('Respuesta completa:', response);
        const token = response.token;
        console.log('Token obtenido:', token);

        if (token) {
          localStorage.setItem('access_token', token);
          this.router.navigate(['/tasks']);
        } else {
          this.errorMessage = 'No se recibió un token válido.';
        }
      },
      error: (err) => {
        console.error('Error al autenticar:', err);
        if (err.error && err.error.message) {
          this.errorMessage = err.error.message;
        } else {
          this.errorMessage = 'Error al autenticar. Intente nuevamente.';
          console.log(this.errorMessage);
        }
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}
