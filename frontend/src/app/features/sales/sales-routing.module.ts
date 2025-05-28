import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SalesListComponent } from './components/sales-list/sales-list.component';
import { SalesFormComponent } from './components/sales-form/sales-form.component';

const routes: Routes = [
  { path: '', component: SalesListComponent },
  { path: 'index', component: SalesListComponent },
  { path: 'edit/:id', component: SalesFormComponent },
  { path: 'create', component: SalesFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule { }
