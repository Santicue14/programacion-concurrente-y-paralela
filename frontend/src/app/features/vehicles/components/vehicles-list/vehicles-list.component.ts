import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { VehicleService } from '../../services/vehicles-api.service';
import { Vehicle } from '../../../../core/models/Vehicle.model';
declare var bootstrap: any; 

@Component({
  selector: 'app-vehicles-list',
  imports:[FormsModule, CommonModule],
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[] = [];
  selectedVehicle: Vehicle | null = null;
  vehicle: Vehicle = {} as Vehicle; // Creamos una tarea vacía para poder usarla en el modal

  constructor(private vehicleService: VehicleService, private router: Router) { }

  ngOnInit(): void {	
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehiculos().subscribe((data: Vehicle[]) => {
      this.vehicles = data;
    });
  }
  
  onCreateVehicle(): void {
    //this.router.navigate(['/tasks/create']);
	this.router.navigate(['/vehicles/create'], { fragment: 'taskFormModal' });
  }
  
  onEditVehicle(vehicle: Vehicle): void {
    console.log(vehicle);
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
          console.log('Tarea borrada:', this.selectedVehicle);
		  // Limpia la tarea seleccionada
          this.selectedVehicle = null; 
          // Recarga las tareas o actualiza la vista
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
