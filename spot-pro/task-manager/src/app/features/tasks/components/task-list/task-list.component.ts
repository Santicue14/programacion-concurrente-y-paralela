import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../services/task-api.service';
import { Task } from '../../../../core/models/task.model';
declare var bootstrap: any; 

@Component({
  selector: 'app-task-list',
  imports:[FormsModule, CommonModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  selectedTask: Task | null = null;
  task: Task = {} as Task; // Creamos una tarea vacía para poder usarla en el modal

  constructor(private taskService: TaskService, private router: Router) { }

  ngOnInit(): void {	
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe((data: Task[]) => {
      this.tasks = data;
    });
  }
  
  onCreateTask(): void {
    //this.router.navigate(['/tasks/create']);
	this.router.navigate(['/tasks/create'], { fragment: 'taskFormModal' });
  }
  
  onEditTask(task: Task): void {
    console.log(task);
	this.router.navigate([`/tasks/edit/${task.id}`], { fragment: 'taskFormModal' });

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
  onDeleteTask(task: Task): void {
    this.selectedTask = task;

    // Obtner el modal y hacer el método show
    const deleteModal = new bootstrap.Modal(
      document.getElementById('deleteTaskModal')
    );
    deleteModal.show();
  }

  // Método para confirmar el borrado
  confirmDelete(): void {
    if (this.selectedTask) {
      this.taskService.deleteTask(this.selectedTask.id).subscribe(
        () => {
          console.log('Tarea borrada:', this.selectedTask);
		  // Limpia la tarea seleccionada
          this.selectedTask = null; 
          // Recarga las tareas o actualiza la vista
          this.loadTasks();
        },
        (error) => {
          console.error('Error al borrar la tarea:', error);
        }
      );
    }

    // Cierra el modal
    const deleteModal = bootstrap.Modal.getInstance(
      document.getElementById('deleteTaskModal')
    );
    deleteModal.hide();
  }
}
