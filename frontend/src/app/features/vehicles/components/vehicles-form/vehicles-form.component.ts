import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { VehicleService } from '../../services/vehicles-api.service';
import { Location } from '@angular/common';  // Para regresar al lugar anterior
import { Vehicle } from '../../../../core/models/Vehicle.model';

import * as $ from 'jquery';
import {Fancybox} from "@fancyapps/ui";

@Component({
  selector: 'app-vehicles-form',
  imports:[FormsModule, CommonModule],
  templateUrl: './vehicles-form.component.html',
  styleUrl: './vehicles-form.component.css'
})

export class VehicleFormComponent implements OnInit {
  vehicle: Vehicle = {
    id: 0,
    marca: '',
    modelo: '',
    anio: 0,
    precio: '',
    stock: 0
  };
  
  isEditMode: boolean = false;

  constructor(
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    //Fancybox.bind("[data-fancybox]", {});
	
	console.log('Iniciando Fancybox...');

    // Usar opciones válidas de Fancybox v4
    Fancybox.bind('[data-fancybox]', {
      // Aquí puedes poner configuraciones permitidas por Fancybox v4
      // Ejemplo: Configuración del 'loop' en las galerías de imágenes
      
      // Mostrar el botón de cerrar
      closeButton: false
    });

    // Escuchar el evento 'fancybox.closed' globalmente
    document.addEventListener('fancybox.closed', () => {
      console.log('Fancybox se ha cerrado.');
      // Actualizar la URL para evitar que el hash se quede en la URL
      window.history.replaceState({}, document.title, window.location.pathname);
      console.log('URL después de cambiar: ', window.location.href);
    });


  
    this.route.fragment.subscribe(fragment => {
      if (fragment === 'vehicleFormModal') {
        this.openModal();
      }
    });
    const vehicleId = this.route.snapshot.paramMap.get('id');
    
    if (vehicleId) {
      this.isEditMode = true;
      this.vehicleService.getVehicleById(+vehicleId).subscribe(
        (vehicle: Vehicle) => {
          this.vehicle = vehicle;
        },
        (error) => {
          console.error('Error fetching vehicle', error);
          this.router.navigate(['/vehicles']); // Redirige a la lista de tareas en caso de error
        }
      );
    }
  }
  
  openModal(): void {
	  document.getElementById('vehicleFormModal')?.style.setProperty('width', '80%');
	  Fancybox.show([
		{
		  src: "#vehicleFormModal",
		  type: "inline",		  
		  height: 'auto',
		  transition: 'fade'
		}
	  ]);
  }

  onSubmit(): void {
	  console.log('Submitting form...');
	  console.log('isEditMode:', this.isEditMode);
	  console.log('Vehicle:', this.vehicle);

	  if (this.isEditMode) {
		console.log('Updating task...');
		this.vehicleService.updateVehicle(this.vehicle.id, this.vehicle).subscribe(
		  (updatedVehicle) => {
			console.log('Updated task:', updatedVehicle);
			Fancybox.close();
			this.router.navigate(['/vehicles']);
		  },
		  (error) => {
			console.error('Error updating task', error);
		  }
		);
	  } else {
		console.log('Creating new task...');
		this.vehicleService.createVehicle(this.vehicle).subscribe(
		  (newVehicle) => {
			console.log('Created task:', newVehicle);
			console.log('Token before navigate:', localStorage.getItem('access_token'));
			this.router.navigate(['/vehicles']);
		  },
		  (error) => {
			console.error('Error creating task', error);
		  }
		);
	  }
	}


  goBack(): void {
	Fancybox.close();
    this.router.navigate(['/tasks']);
  }
}