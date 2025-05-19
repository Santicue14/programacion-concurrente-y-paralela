import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports:[FormsModule, CommonModule, HttpClientModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

	// private http: HttpClient, 
	
	user = {
		username: '',
		password: '',
		confirmPassword: ''
	  };
    errorMessage: string = '';
  
	constructor(private authService: AuthService, private router: Router) { }
	
	
	
	onSubmit() {
    if (this.user.password === this.user.confirmPassword) {
      // Aquí debes llamar a la API para registrar al usuario
      this.authService.register(this.user).subscribe(
        (response) => {
          // Si el registro es exitoso, redirige al login
          this.router.navigate(['/login']);
        },
        (error) => {
          console.error('Error during registration', error);
		  this.errorMessage = error.message || 'Error al registrarse.';
        }
      );
    } else {
      alert('Passwords do not match');
    }
  }

}
