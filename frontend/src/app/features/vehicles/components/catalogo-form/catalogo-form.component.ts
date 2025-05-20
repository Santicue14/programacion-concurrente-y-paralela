import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';

import { VehicleService } from '../../services/vehicles-api.service';
import { Location } from '@angular/common';  // Para regresar al lugar anterior

import * as $ from 'jquery';
import {Fancybox} from "@fancyapps/ui";

@Component({
  selector: 'app-catalogo-form',
  templateUrl: './catalogo-form.component.html',
  styleUrls: ['./catalogo-form.component.css'],
  imports: [FormsModule, CommonModule, NgSelectModule],
  standalone: true
})

export class CatalogoFormComponent implements OnInit {
  marcas: any[] = [];
  marca: string = '';
  modelos: any[] = [];
  modelo: string = '';
  filteredModelos: any[] = [];

  isEditMode: boolean = false;
  type: 'marca' | 'modelo' = 'marca';
  formData: any = {
    nombre: '',
    marcaId: ''
  };

  constructor(
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router
  ) {}


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
    })
  

    this.type = this.route.snapshot.data['type'];
    if (this.type === 'modelo') {
      this.loadMarcas();
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
    this.modelo = ''; // Resetear el modelo cuando cambia la marca
    this.updateFilteredModelos();
  }

  updateFilteredModelos(): void {
    if (this.marca) {
      this.filteredModelos = this.modelos.filter(
        m => m.marcaId == this.marca
      )
    } else {
      this.filteredModelos = [];
    }
  }

  onSubmitForm(): void {
    if (this.type === 'marca') {
      this.vehicleService.createMarca(this.formData).subscribe({
        next: () => {
          this.router.navigate(['/vehicles']);
        },
        error: (error) => {
          console.error('Error al crear marca:', error);
        }
      });
    } else {
      this.vehicleService.createModelo(this.formData).subscribe({
        next: () => {
          this.router.navigate(['/vehicles']);
        },
        error: (error) => {
          console.error('Error al crear modelo:', error);
        }
      });
    }
  }
}