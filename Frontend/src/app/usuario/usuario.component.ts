import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UsuarioService } from '../services/usuario.service';

@Component({
  selector: 'app-usuario',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './usuario.component.html',
  styleUrl: './usuario.component.css',
})
export class UsuarioComponent implements OnInit {
  usuarioForm: FormGroup;
  isEditMode = signal(false);
  usuarioId?: number;
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.usuarioForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      fechaNacimiento: ['', Validators.required],
      sexo: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.isEditMode.set(true);
        this.usuarioId = +params['id'];
        this.cargarUsuario();
      }
    });
  }

  cargarUsuario(): void {
    if (this.usuarioId) {
      this.loading.set(true);
      this.usuarioService.getUsuario(this.usuarioId).subscribe({
        next: (usuario) => {
          this.usuarioForm.patchValue(usuario);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set('Error al cargar el usuario');
          this.loading.set(false);
          console.error(err);
        },
      });
    }
  }

  onSubmit(): void {
    if (this.usuarioForm.valid) {
      this.loading.set(true);
      this.error.set(null);
      this.success.set(null);

      const usuario = this.usuarioForm.value;

      if (this.isEditMode() && this.usuarioId) {
        this.usuarioService.updateUsuario(this.usuarioId, usuario).subscribe({
          next: () => {
            this.success.set('Usuario actualizado correctamente');
            this.loading.set(false);
            setTimeout(() => {
              this.router.navigate(['/consulta']);
            }, 1500);
          },
          error: (err) => {
            this.error.set('Error al actualizar el usuario');
            this.loading.set(false);
            console.error(err);
          },
        });
      } else {
        this.usuarioService.createUsuario(usuario).subscribe({
          next: () => {
            this.success.set('Usuario creado correctamente');
            this.loading.set(false);
            this.usuarioForm.reset();
            setTimeout(() => {
              this.router.navigate(['/consulta']);
            }, 1500);
          },
          error: (err) => {
            this.error.set('Error al crear el usuario');
            this.loading.set(false);
            console.error(err);
          },
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/consulta']);
  }
}
