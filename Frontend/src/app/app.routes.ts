import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/consulta', pathMatch: 'full' },
  { 
    path: 'usuario', 
    loadComponent: () => import('./usuario/usuario.component').then(m => m.UsuarioComponent)
  },
  { 
    path: 'usuario/:id', 
    loadComponent: () => import('./usuario/usuario.component').then(m => m.UsuarioComponent)
  },
  { 
    path: 'consulta', 
    loadComponent: () => import('./usuario-consulta/usuario-consulta.component').then(m => m.UsuarioConsultaComponent)
  },
  { path: '**', redirectTo: '/consulta' }
];
