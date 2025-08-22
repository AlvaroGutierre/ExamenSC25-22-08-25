import { Routes } from '@angular/router';

import { PeliculasComponent } from './peliculas.component';
export const routes: Routes = [
  { path: '', component: PeliculasComponent },
  {
    path: 'detalle/:id',
    // @ts-ignore
    loadComponent: () =>
      import('./detalle/detalle.component').then((m) => m.DetalleComponent),
  },
];
