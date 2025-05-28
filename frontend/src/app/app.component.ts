import { RouterOutlet } from '@angular/router';
import { Component, AfterViewInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AuthService } from './features/auth/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements AfterViewInit {
  title = 'task-manager';
  
  constructor(public authService: AuthService, private router: Router) { }

  ngAfterViewInit() {
    // Add mobile dropdown toggle functionality
    this.setupMobileDropdowns();
  }

  setupMobileDropdowns() {
    document.addEventListener('click', (event: Event) => {
      if (window.innerWidth <= 768) {
        const target = event.target as HTMLElement;
        if (target && target.classList.contains('dropdown-toggle')) {
          event.preventDefault();
          const parent = target.closest('.dropdown');
          if (parent) {
            parent.classList.toggle('show');
          }
        }
      }
    });
  }

  // Método para llamar al logout
  logout() {
    this.authService.logout().subscribe({
      next: (response) => {
        localStorage.removeItem('access_token');  // Elimina el token del localStorage
        this.router.navigate(['/login']);  // Redirige al login
      },
      error: (err) => {
        console.error('Error during logout', err);
        this.router.navigate(['/login']);  // Redirige al login en caso de error
      }
    });
  } 
}
