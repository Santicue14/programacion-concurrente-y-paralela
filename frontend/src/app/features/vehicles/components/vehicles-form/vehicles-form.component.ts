import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { VehicleService } from '../../services/vehicles-api.service';
import { Location } from '@angular/common';  // Para regresar al lugar anterior
import { Vehicle } from '../../../../core/models/vehicle.model';

import * as $ from 'jquery';
import {Fancybox} from "@fancyapps/ui";

@Component({
  selector: 'app-vehicles-form',
  templateUrl: './vehicles-form.component.html',
  styleUrls: ['./vehicles-form.component.css'],
  imports: [FormsModule, CommonModule, NgSelectModule],
  standalone: true
})

export class VehicleFormComponent implements OnInit {
  marcas: any[] = [];
  modelos: any[] = [];
  filteredModelos: any[] = [];
  years: number[] = [];
  vehicle: Vehicle = {
    id: 0,
    marca: '',
    modelo: '',
    anio: 0,
    precio: '',
    stock: 0
  };
  
  isEditMode: boolean = false;
private _: any;

  constructor(
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.generateYears();
  }

  private generateYears(): void {
    const currentYear = new Date().getFullYear();
    this.years = Array.from(
      { length: currentYear - 1959 },
      (_, i) => currentYear - i
    );
  }

  ngOnInit(): void {
	this.loadMarcas();
	this.loadModelos();
	
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
    // Obtener los nombres de marca y modelo para enviar al backend
    const modeloSeleccionado = this.modelos.find(m => m.id == this.vehicle.modelo);
    const marcaSeleccionada = this.marcas.find(m => m.id == this.vehicle.marca);

    // Crear objeto a enviar al backend
    const vehiculoDTO: any = {
      id: this.vehicle.id,
      anio: this.vehicle.anio,
      precio: typeof this.vehicle.precio === 'string' ? 
              parseFloat(this.vehicle.precio.replace(/,/g, '')) : 
              this.vehicle.precio,
      stock: this.vehicle.stock,
      ModeloId: parseInt(this.vehicle.modelo.toString())
    };

    if (this.isEditMode) {
      console.log('Updating vehicle...', vehiculoDTO);
      this.vehicleService.updateVehicle(this.vehicle.id, vehiculoDTO).subscribe(
        (updatedVehicle) => {
          console.log('Updated vehicle:', updatedVehicle);
          Fancybox.close();
          this.router.navigate(['/vehicles']);
        },
        (error) => {
          console.error('Error updating vehicle', error);
        }
      );
    } else {
      console.log('Creating new vehicle...', vehiculoDTO);
      this.vehicleService.createVehicle(vehiculoDTO).subscribe(
        (newVehicle) => {
          console.log('Created vehicle:', newVehicle);
          this.router.navigate(['/vehicles']);
        },
        (error) => {
          console.error('Error creating vehicle', error);
        }
      );
    }
  }

  goBack(): void {
	Fancybox.close();
    this.router.navigate(['/vehicles']);
  }

  loadMarcas(): void {
    this.vehicleService.getMarcas().subscribe({
      next: (data) => {
        this.marcas = data;
      },
      error: (error) => {
        console.error('Error al cargar marcas:', error);
      }
    });
  }

  loadModelos(): void {
    this.vehicleService.getModelos().subscribe({
      next: (data) => {
        this.modelos = data;
        this.updateFilteredModelos();
      },
      error: (error) => {
        console.error('Error al cargar modelos:', error);
      }
    });
  }

  onMarcaChange(): void {
    this.vehicle.modelo = ''; // Resetear el modelo cuando cambia la marca
    this.updateFilteredModelos();
  }

  updateFilteredModelos(): void {
    if (this.vehicle.marca) {
      this.filteredModelos = this.modelos.filter(
        modelo => modelo.marcaId == this.vehicle.marca
      )
    } else {
      this.filteredModelos = [];
    }
  }
}