import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports:[FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  onLogin(): void {
  this.authService.login(this.username, this.password).subscribe({
    next: (response) => {
      console.log('Respuesta completa:', response);
      const token = response.access_token;
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
      this.errorMessage = 'Error al autenticar. Intente nuevamente.';
    }
  });
}


}
