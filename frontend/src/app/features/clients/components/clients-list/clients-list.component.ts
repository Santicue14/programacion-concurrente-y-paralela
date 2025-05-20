import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClientService } from '../../services/clients-api.service';
import { Client } from '../../../../core/models/client.model';

@Component({
  selector: 'app-clients-list',
  templateUrl: './clients-list.component.html',
  styleUrls: ['./clients-list.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class ClientListComponent implements OnInit {
  clients: Client[] = [];
  loading = true;
  error = false;

  constructor(private clientService: ClientService) { }

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.loading = true;
    this.error = false;
    
    this.clientService.getClients().subscribe({
      next: (data) => {
        this.clients = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading clients', error);
        this.error = true;
        this.loading = false;
      }
    });
  }

  deleteClient(id: number): void {
    if (confirm('¿Está seguro que desea eliminar este cliente?')) {
      this.clientService.deleteClient(id).subscribe({
        next: () => {
          this.clients = this.clients.filter(client => client.id !== id);
        },
        error: (error) => {
          console.error('Error deleting client', error);
        }
      });
    }
  }
} 