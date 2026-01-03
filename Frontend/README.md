# Frontend - Sistema de Gestión de Usuarios

Aplicación Angular 20.3.0 que consume la API REST del backend de gestión de usuarios.

## 🚀 Características

- **Angular 20.3.0** con Zoneless Change Detection
- **Tailwind CSS** para estilos modernos y responsivos
- **Lazy Loading** para optimizar la carga de componentes
- **Reactive Forms** para validación de formularios
- **HttpClient** con fetch API
- **Signals** para manejo de estado reactivo

## 📋 Páginas Implementadas

### 1. Usuario Consulta (`/consulta`)

- Lista todos los usuarios registrados en una grilla
- Botones de **Modificar** y **Eliminar** para cada usuario
- Modal de confirmación para eliminación
- Indicadores visuales con badges para el sexo
- Estado vacío con mensaje cuando no hay usuarios

### 2. Usuario Formulario (`/usuario`)

- **Campo Nombre**: Input de texto (requerido, máx. 100 caracteres)
- **Campo Fecha de Nacimiento**: Input tipo date (requerido)
- **Campo Sexo**: Dropdown con opciones Masculino/Femenino (requerido)
- Validaciones en tiempo real
- Mensajes de éxito/error
- Modo creación y edición (`/usuario/:id`)

## 🏗️ Estructura del Proyecto

```
src/
├── app/
│   ├── models/
│   │   └── usuario.model.ts          # Interfaces y DTOs
│   ├── services/
│   │   └── usuario.service.ts        # Servicio HTTP para API
│   ├── usuario/
│   │   ├── usuario.component.ts      # Formulario de usuario
│   │   ├── usuario.component.html
│   │   └── usuario.component.css
│   ├── usuario-consulta/
│   │   ├── usuario-consulta.component.ts  # Grilla de usuarios
│   │   ├── usuario-consulta.component.html
│   │   └── usuario-consulta.component.css
│   ├── app.config.ts                 # Configuración global
│   ├── app.routes.ts                 # Rutas de la aplicación
│   ├── app.ts                        # Componente raíz
│   ├── app.html
│   └── app.css
├── styles.css                        # Estilos globales + Tailwind
└── index.html
```

## ⚙️ Instalación y Ejecución

### Prerrequisitos

- Node.js 18+ y npm
- Angular CLI 20.3.0

### Instalar dependencias

```bash
npm install
```

### Ejecutar en modo desarrollo

```bash
npm start
```

La aplicación estará disponible en `http://localhost:4200/`

### Compilar para producción

```bash
npm run build
```

Los archivos compilados estarán en `dist/frontend/browser/`

## 🔗 Configuración de la API

La aplicación está configurada para consumir la API del backend en:

```typescript
// src/app/services/usuario.service.ts
private apiUrl = 'https://localhost:7254/api/usuarios';
```

**Importante**: Asegúrate de que el backend esté ejecutándose antes de usar la aplicación.

## 📡 Endpoints Consumidos

| Método | Endpoint             | Descripción                  |
| ------ | -------------------- | ---------------------------- |
| GET    | `/api/usuarios`      | Obtener todos los usuarios   |
| GET    | `/api/usuarios/{id}` | Obtener usuario por ID       |
| POST   | `/api/usuarios`      | Crear nuevo usuario          |
| PUT    | `/api/usuarios/{id}` | Actualizar usuario existente |
| DELETE | `/api/usuarios/{id}` | Eliminar usuario             |

## 🎨 Tailwind CSS

El proyecto utiliza Tailwind CSS 3.4.17 para los estilos. La configuración está en:

- `tailwind.config.js` - Configuración de Tailwind
- `src/styles.css` - Directivas de Tailwind

### Características de UI

- Diseño responsivo (mobile-first)
- Gradientes y colores modernos
- Animaciones suaves
- Componentes accesibles
- Loading states
- Mensajes de éxito/error

## 🔀 Rutas

| Ruta           | Componente      | Descripción              |
| -------------- | --------------- | ------------------------ |
| `/`            | (redirect)      | Redirige a `/consulta`   |
| `/consulta`    | UsuarioConsulta | Lista de usuarios        |
| `/usuario`     | Usuario         | Crear nuevo usuario      |
| `/usuario/:id` | Usuario         | Editar usuario existente |
| `/**`          | (redirect)      | Redirige a `/consulta`   |

## 🧪 Scripts Disponibles

```bash
# Iniciar servidor de desarrollo
npm start

# Compilar para producción
npm run build

# Ejecutar pruebas
npm test

# Ejecutar compilación en modo watch
npm run watch
```

## 🛠️ Tecnologías Utilizadas

- **Angular** 20.3.0
- **TypeScript** 5.9.2
- **Tailwind CSS** 3.4.17
- **RxJS** 7.8.0
- **Angular Router** (lazy loading)
- **Angular Forms** (reactive forms)
- **Angular HttpClient** (con fetch API)

## 📝 Validaciones Implementadas

### Campo Nombre

- Requerido
- Máximo 100 caracteres

### Campo Fecha de Nacimiento

- Requerido
- Formato de fecha válido

### Campo Sexo

- Requerido
- Solo acepta 'M' (Masculino) o 'F' (Femenino)

## 🎯 Características Avanzadas

- **Lazy Loading**: Los componentes se cargan bajo demanda
- **Signals**: Estado reactivo sin Zone.js
- **Standalone Components**: Sin necesidad de NgModules
- **Control Flow Syntax**: Nueva sintaxis `@if`, `@for`
- **Modal de Confirmación**: Para operaciones destructivas
- **Manejo de Errores**: Mensajes claros al usuario
- **Loading States**: Indicadores de carga durante operaciones

## 🐛 Troubleshooting

### Error de CORS

Si ves errores de CORS, verifica que el backend tenga configurado el origen correcto:

```csharp
// Backend: Program.cs
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### Error de conexión a la API

- Verifica que el backend esté ejecutándose en `https://localhost:7254`
- Comprueba que SQL Server esté disponible
- Revisa la consola del navegador para más detalles

## 📄 Licencia

Este proyecto es parte de un sistema de gestión de usuarios con arquitectura por capas.

---

**Desarrollado con Angular 20 y Tailwind CSS**
