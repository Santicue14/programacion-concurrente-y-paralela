import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';

import { SaleService } from '../../services/sales-api.service';
import { ClientService } from '../../../clients/services/clients-api.service';
import { VehicleService } from '../../../vehicles/services/vehicles-api.service';
import { Location } from '@angular/common';

import { Sale } from '../../../../core/models/sale.model';
import { Client } from '../../../../core/models/client.model';
import { Vehicle } from '../../../../core/models/vehicle.model';

@Component({
  selector: 'app-sales-form',
  templateUrl: './sales-form.component.html',
  styleUrls: ['./sales-form.component.css'],
  imports: [FormsModule, CommonModule, NgSelectModule],
  standalone: true
})

export class SalesFormComponent implements OnInit {
  clients: Client[] = [];
  vehicles: Vehicle[] = [];
  isEditMode: boolean = false;
  saleId: number | null = null;
  
  formData: Sale = {
    id: 0,
    cliente: undefined,
    vehiculo: undefined,
    fecha: new Date(),
    total: 0
  };

  // Form fields for IDs
  selectedClientId: number | null = null;
  selectedVehicleId: number | null = null;

  constructor(
    private saleService: SaleService,
    private clientService: ClientService,
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Verificar si es modo edición
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.saleId = +params['id'];
        this.loadSaleData(this.saleId);
      }
    });

    // Cargar datos necesarios
    this.loadClients();
    this.loadVehicles();
  }

  loadClients(): void {
    this.clientService.getClients().subscribe({
      next: (data) => {
        this.clients = data;
      },
      error: (error) => {
        console.error('Error al cargar clientes:', error);
      }
    });
  }

  loadVehicles(): void {
    this.vehicleService.getVehiculos().subscribe({
      next: (data) => {
        this.vehicles = data;
      },
      error: (error) => {
        console.error('Error al cargar vehículos:', error);
      }
    });
  }

  loadSaleData(id: number): void {
    this.saleService.getVentaById(id).subscribe({
      next: (sale) => {
        this.formData = { ...sale };
        this.selectedClientId = sale.cliente?.id || null;
        this.selectedVehicleId = sale.vehiculo?.id || null;
      },
      error: (error) => {
        console.error('Error al cargar venta:', error);
      }
    });
  }

  onClientChange(): void {
    if (this.selectedClientId) {
      this.formData.cliente = this.clients.find(c => c.id === this.selectedClientId);
    } else {
      this.formData.cliente = undefined;
    }
    this.calculateTotal();
  }

  onVehicleChange(): void {
    if (this.selectedVehicleId) {
      this.formData.vehiculo = this.vehicles.find(v => v.id === this.selectedVehicleId);
    } else {
      this.formData.vehiculo = undefined;
    }
    this.calculateTotal();
  }

  calculateTotal(): void {
    if (this.formData.vehiculo && this.formData.vehiculo.precio) {
      // Convert precio to number if it's a string
      const precio = typeof this.formData.vehiculo.precio === 'string' 
        ? parseFloat(this.formData.vehiculo.precio) 
        : this.formData.vehiculo.precio;
      
      this.formData.total = precio || 0;
    } else {
      this.formData.total = 0;
    }
  }

  onSubmit(): void {
    if (!this.isFormValid()) {
      console.error('Formulario inválido');
      return;
    }

    // Preparar datos para envío
    const saleData = {
      id: this.formData.id,
      clienteId: this.selectedClientId,
      vehiculoId: this.selectedVehicleId,
      fecha: this.formData.fecha,
      total: this.formData.total
    };

    if (this.isEditMode && this.saleId) {
      // Actualizar venta existente
      this.saleService.updateVenta(this.saleId, saleData).subscribe({
        next: () => {
          console.log('Venta actualizada exitosamente');
          this.goBack();
        },
        error: (error: any) => {
          console.error('Error al actualizar venta:', error);
        }
      });
    } else {
      // Crear nueva venta
      this.saleService.createVenta(saleData).subscribe({
        next: () => {
          console.log('Venta creada exitosamente');
          this.goBack();
        },
        error: (error: any) => {
          console.error('Error al crear venta:', error);
        }
      });
    }
  }

  isFormValid(): boolean {
    return !!(
      this.selectedClientId &&
      this.selectedVehicleId &&
      this.formData.fecha &&
      this.formData.total > 0
    );
  }

  goBack(): void {
    this.router.navigate(['/sales']);
  }

  // Getter para el título del formulario
  get formTitle(): string {
    return this.isEditMode ? 'Editar Venta' : 'Nueva Venta';
  }

  // Getter para el texto del botón
  get submitButtonText(): string {
    return this.isEditMode ? 'Actualizar Venta' : 'Crear Venta';
  }
}