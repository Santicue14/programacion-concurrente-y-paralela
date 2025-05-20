import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleListComponent } from './components/vehicles-list/vehicles-list.component';
import { VehicleFormComponent } from './components/vehicles-form/vehicles-form.component';

const routes: Routes = [
  { path: '', component: VehicleListComponent },
  { path: 'index', component: VehicleListComponent },
  { path: 'edit/:id', component: VehicleFormComponent },
  { path: 'create', component: VehicleFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehiclesRoutingModule { }
