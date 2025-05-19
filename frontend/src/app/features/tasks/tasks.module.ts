import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms'; 
import { TasksRoutingModule } from './tasks-routing.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TasksRoutingModule,
	ReactiveFormsModule
  ]
})
export class TasksModule { }
