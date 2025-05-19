import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../services/task-api.service';
import { Location } from '@angular/common';  // Para regresar al lugar anterior
import { Task } from '../../../../core/models/task.model';

import * as $ from 'jquery';
import {Fancybox} from "@fancyapps/ui";

@Component({
  selector: 'app-task-form',
  imports:[FormsModule, CommonModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css'
})

export class TaskFormComponent implements OnInit {
  task: Task = {
    id: 0,
    title: '',
    description: '',
    completed: false,
    due_date: '',
    user_id: 1 // Ajusta esto según el contexto de tu aplicación
  };
  
  isEditMode: boolean = false;

  constructor(
    private taskService: TaskService,
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
      if (fragment === 'taskFormModal') {
        this.openModal();
      }
    });
    const taskId = this.route.snapshot.paramMap.get('id');
    
    if (taskId) {
      this.isEditMode = true;
      this.taskService.getTaskById(+taskId).subscribe(
        (task: Task) => {
          this.task = task;
		  if (this.task.due_date) {
			this.task.due_date = new Date(this.task.due_date).toISOString().slice(0, 16);
		  }
        },
        (error) => {
          console.error('Error fetching task', error);
          this.router.navigate(['/tasks']); // Redirige a la lista de tareas en caso de error
        }
      );
    }
  }
  
  openModal(): void {
	  document.getElementById('taskFormModal')?.style.setProperty('width', '80%');
	  Fancybox.show([
		{
		  src: "#taskFormModal",
		  type: "inline",		  
		  height: 'auto',
		  transition: 'fade'
		}
	  ]);
  }

  onSubmit(): void {
	  console.log('Submitting form...');
	  console.log('isEditMode:', this.isEditMode);
	  console.log('Task:', this.task);

	  if (this.isEditMode) {
		console.log('Updating task...');
		this.taskService.updateTask(this.task.id, this.task).subscribe(
		  (updatedTask) => {
			console.log('Updated task:', updatedTask);
			Fancybox.close();
			this.router.navigate(['/tasks']);
		  },
		  (error) => {
			console.error('Error updating task', error);
		  }
		);
	  } else {
		console.log('Creating new task...');
		this.taskService.createTask(this.task).subscribe(
		  (newTask) => {
			console.log('Created task:', newTask);
			console.log('Token before navigate:', localStorage.getItem('access_token'));
			this.router.navigate(['/tasks']);
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