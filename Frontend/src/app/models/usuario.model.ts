export interface Usuario {
  id?: number;
  nombre: string;
  fechaNacimiento: string;
  sexo: 'M' | 'F';
}

export interface UsuarioCreateDto {
  nombre: string;
  fechaNacimiento: string;
  sexo: 'M' | 'F';
}

export interface UsuarioUpdateDto {
  nombre: string;
  fechaNacimiento: string;
  sexo: 'M' | 'F';
}
