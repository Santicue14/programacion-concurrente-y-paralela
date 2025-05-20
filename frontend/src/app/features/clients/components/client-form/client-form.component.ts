import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClientService } from '../../services/clients-api.service';
import { Client } from '../../../../core/models/client.model';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule]
})
export class ClientFormComponent implements OnInit {
  client: Client = {
    id: 0,
    nombre: '',
    apellido: '',
    email: '',
    telefono: ''
  };
  
  isEditMode: boolean = false;
  loading: boolean = false;
  submitted: boolean = false;
  error: string = '';

  constructor(
    private clientService: ClientService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const clientId = this.route.snapshot.paramMap.get('id');
    
    if (clientId) {
      this.isEditMode = true;
      this.loading = true;
      
      this.clientService.getClientById(+clientId).subscribe({
        next: (client) => {
          this.client = client;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error fetching client', error);
          this.loading = false;
          this.error = 'No se pudo cargar la información del cliente';
          this.router.navigate(['/clients']);
        }
      });
    }
  }

  onSubmit(): void {
    this.submitted = true;
    this.loading = true;
    this.error = '';

    if (this.isEditMode) {
      this.clientService.updateClient(this.client.id, this.client).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/clients']);
        },
        error: (error) => {
          console.error('Error updating client', error);
          this.loading = false;
          this.error = 'Error al actualizar el cliente';
        }
      });
    } else {
      this.clientService.createClient(this.client).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/clients']);
        },
        error: (error) => {
          console.error('Error creating client', error);
          this.loading = false;
          this.error = 'Error al crear el cliente';
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/clients']);
  }
} 