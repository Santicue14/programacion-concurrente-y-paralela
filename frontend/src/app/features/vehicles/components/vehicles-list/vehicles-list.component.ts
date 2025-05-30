import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { VehicleService } from '../../services/vehicles-api.service';
import { Vehicle } from '../../../../core/models/vehicle.model';
declare var bootstrap: any; 

@Component({
  selector: 'app-vehicles-list',
  imports:[FormsModule, CommonModule],
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[] = [];
  filteredVehicles: Vehicle[] = [];
  paginatedVehicles: Vehicle[] = [];
  marcas: string[] = [];
  
  // Paginación
  currentPage = 1;
  itemsPerPage = 10;
  totalPages = 1;
  
  // Filtros
  searchTerm = '';
  selectedMarca = '';

  // Ordenamiento
  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  selectedVehicle: Vehicle | null = null;
  vehicle: Vehicle = {} as Vehicle; // Creamos un vehículo vacío para poder usarlo en el modal

  constructor(private vehicleService: VehicleService, private router: Router) { }

  ngOnInit(): void {	
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehiculos().subscribe({
      next: (data) => {
        this.vehicles = data;
        this.marcas = [...new Set(data.map(v => v.marca))].filter((marca): marca is string => typeof marca === 'string');
        this.filterVehicles();
      },
      error: (error) => {
        console.error('Error al cargar vehículos:', error);
      }
    });
  }

  sortBy(column: string): void {
    if (this.sortColumn === column) {
      // Si ya está ordenado por esta columna, cambia la dirección
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // Si es una nueva columna, ordena ascendente por defecto
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.filterVehicles();
  }

  filterVehicles(): void {
    // Primero aplica los filtros
    this.filteredVehicles = this.vehicles.filter(vehicle => {
      const matchesSearch = !this.searchTerm || 
        (typeof vehicle.marca === 'string' && vehicle.marca.toLowerCase().includes(this.searchTerm.toLowerCase())) ||
        (typeof vehicle.modelo === 'string' && vehicle.modelo.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      const matchesMarca = !this.selectedMarca || 
        vehicle.marca === this.selectedMarca;

      return matchesSearch && matchesMarca;
    });

    // Luego aplica el ordenamiento
    if (this.sortColumn) {
      this.filteredVehicles.sort((a, b) => {
        let valueA = a[this.sortColumn as keyof Vehicle];
        let valueB = b[this.sortColumn as keyof Vehicle];

        // Manejo especial para valores booleanos (stock)
        if (typeof valueA === 'boolean' && typeof valueB === 'boolean') {
          return this.sortDirection === 'asc' 
            ? (valueA === valueB ? 0 : valueA ? -1 : 1)
            : (valueA === valueB ? 0 : valueA ? 1 : -1);
        }

        // Manejo especial para números (precio)
        if (typeof valueA === 'number' && typeof valueB === 'number') {
          return this.sortDirection === 'asc' 
            ? valueA - valueB 
            : valueB - valueA;
        }

        // Para strings (marca, modelo)
        const stringA = String(valueA).toLowerCase();
        const stringB = String(valueB).toLowerCase();
        
        return this.sortDirection === 'asc'
          ? stringA.localeCompare(stringB)
          : stringB.localeCompare(stringA);
      });
    }

    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredVehicles.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    this.paginatedVehicles = this.filteredVehicles.slice(
      startIndex,
      startIndex + this.itemsPerPage
    );
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    
    if (this.totalPages <= maxPagesToShow) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
      let endPage = startPage + maxPagesToShow - 1;
      
      if (endPage > this.totalPages) {
        endPage = this.totalPages;
        startPage = Math.max(1, endPage - maxPagesToShow + 1);
      }
      
      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }
    
    return pages;
  }

  onCreateVehicle(): void {
    //this.router.navigate(['/tasks/create']);
	this.router.navigate(['/vehicles/create'], { fragment: 'taskFormModal' });
  }
  
  onEditVehicle(vehicle: Vehicle): void {
	this.router.navigate([`/tasks/edit/${vehicle.id}`], { fragment: 'taskFormModal' });

	// Llamar a Fancybox manualmente
	// Fancybox.show([
	  // {
		// src: "#taskFormModal",
		// type: "inline",
	  // },
	// ]);
	//this.router.navigate([`/tasks/edit/${task.id}`]);
  }     
  
  // Método para abrir el modal para tratar la eliminación
  onDeleteVehicle(vehicle: Vehicle): void {
    this.selectedVehicle = vehicle;

    // Obtner el modal y hacer el método show
    const deleteModal = new bootstrap.Modal(
      document.getElementById('deleteVehicleModal')
    );
    deleteModal.show();
  }

  // Método para confirmar el borrado
  confirmDelete(): void {
    if (this.selectedVehicle) {
      this.vehicleService.deleteVehicle(this.selectedVehicle.id).subscribe(
        () => {
		  // Limpia el vehículo seleccionado
          this.selectedVehicle = null; 
          // Recarga los vehículos o actualiza la vista
          this.loadVehicles();
        },
        (error) => {
          console.error('Error al borrar la tarea:', error);
        }
      );
    }

    // Cierra el modal
    const deleteModal = bootstrap.Modal.getInstance(
      document.getElementById('deleteVehicleModal')
    );
    deleteModal.hide();
  }
}
