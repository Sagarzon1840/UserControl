import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UsuarioService } from '../services/usuario.service';
import { Usuario } from '../models/usuario.model';

@Component({
  selector: 'app-usuario-consulta',
  imports: [CommonModule, RouterModule],
  templateUrl: './usuario-consulta.component.html',
  styleUrl: './usuario-consulta.component.css',
})
export class UsuarioConsultaComponent implements OnInit {
  usuarios = signal<Usuario[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);
  usuarioAEliminar: Usuario | null = null;
  mostrarModal = signal(false);

  constructor(private usuarioService: UsuarioService, private router: Router) {}

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios(): void {
    this.loading.set(true);
    this.error.set(null);
    this.usuarioService.getUsuarios().subscribe({
      next: (usuarios) => {
        this.usuarios.set(usuarios);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error al cargar los usuarios');
        this.loading.set(false);
        console.error(err);
      },
    });
  }

  editarUsuario(id?: number): void {
    if (id) {
      this.router.navigate(['/usuario', id]);
    }
  }

  confirmarEliminar(usuario: Usuario): void {
    this.usuarioAEliminar = usuario;
    this.mostrarModal.set(true);
  }

  cancelarEliminar(): void {
    this.usuarioAEliminar = null;
    this.mostrarModal.set(false);
  }

  eliminarUsuario(): void {
    if (this.usuarioAEliminar?.id) {
      this.loading.set(true);
      this.usuarioService.deleteUsuario(this.usuarioAEliminar.id).subscribe({
        next: () => {
          this.success.set('Usuario eliminado correctamente');
          this.mostrarModal.set(false);
          this.usuarioAEliminar = null;
          this.cargarUsuarios();
          setTimeout(() => this.success.set(null), 3000);
        },
        error: (err) => {
          this.error.set('Error al eliminar el usuario');
          this.loading.set(false);
          this.mostrarModal.set(false);
          console.error(err);
        },
      });
    }
  }

  getSexoTexto(sexo: string): string {
    return sexo === 'M' ? 'Masculino' : 'Femenino';
  }

  formatearFecha(fecha: string): string {
    const date = new Date(fecha);
    return date.toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  }
}
