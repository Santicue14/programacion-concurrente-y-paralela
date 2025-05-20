import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './features/auth/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'vehicles',
    loadChildren: () =>
      import('./features/vehicles/vehicles.module').then((m) => m.VehiclesModule),  
    canActivate: [AuthGuard],
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'clients',
    loadChildren: () =>
      import('./features/clients/clients.module').then((m) => m.ClientsModule),
    canActivate: [AuthGuard],
  },
  {
    path: '**',
    redirectTo: 'vehicles',//Ruta por defecto
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
