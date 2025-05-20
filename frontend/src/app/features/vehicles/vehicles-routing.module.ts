import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleListComponent } from './components/vehicles-list/vehicles-list.component';
import { VehicleFormComponent } from './components/vehicles-form/vehicles-form.component';
import { CatalogoFormComponent } from './components/catalogo-form/catalogo-form.component';

const routes: Routes = [
  { path: '', component: VehicleListComponent },
  { path: 'index', component: VehicleListComponent },
  { path: 'edit/:id', component: VehicleFormComponent },
  { path: 'create', component: VehicleFormComponent },
  { path: 'catalogo/marca', component: CatalogoFormComponent, data: { type: 'marca' } },
  { path: 'catalogo/modelo', component: CatalogoFormComponent, data: { type: 'modelo' } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VehiclesRoutingModule { }
