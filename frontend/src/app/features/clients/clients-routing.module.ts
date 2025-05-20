import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientListComponent } from './components/clients-list/clients-list.component';
import { ClientFormComponent } from './components/client-form/client-form.component';

const routes: Routes = [
  { path: '', component: ClientListComponent },
  { path: 'index', component: ClientListComponent },
  { path: 'edit/:id', component: ClientFormComponent },
  { path: 'create', component: ClientFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientsRoutingModule { } 