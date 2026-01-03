# Sistema de Gestión de Usuarios - Full Stack

Sistema completo de gestión de usuarios con arquitectura por capas en el backend (.NET 8) y frontend moderno con Angular 20.

## 📋 Tabla de Contenidos

- [Descripción General](#descripción-general)
- [Arquitectura del Sistema](#arquitectura-del-sistema)
- [Backend - API REST](#backend---api-rest)
- [Frontend - Angular](#frontend---angular)
- [Instalación y Ejecución](#instalación-y-ejecución)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Funcionalidades](#funcionalidades)

---

## 📖 Descripción General

Sistema de gestión de usuarios que permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) con una arquitectura moderna y escalable:

- **Backend**: API REST con .NET 8, arquitectura por capas, ADO.NET y Stored Procedures
- **Frontend**: Aplicación Angular 20 con Tailwind CSS, Signals y Reactive Forms
- **Base de Datos**: SQL Server con Entity Framework Core (solo para migraciones)

## 🏗️ Arquitectura del Sistema

```
UserControl/
├── Backend/                          # API REST .NET 8
│   └── Business.Api/
│       ├── Controllers/              # Endpoints REST
│       ├── Application/              # Lógica de negocio
│       │   ├── Services/
│       │   └── DTOs/
│       ├── Domain/                   # Entidades del dominio
│       │   └── Entities/
│       ├── Outbound/                 # Adaptadores de salida
│       │   └── Persistence/          # Repositorios ADO.NET
│       ├── Data/                     # DbContext y migraciones
│       │   ├── UsuariosDbContext.cs
│       │   └── Migrations/
│       └── Filters/                  # Filtros de validación
│
└── Frontend/                         # Angular 20
    └── src/
        └── app/
            ├── models/               # Interfaces y DTOs
            ├── services/             # Servicios HTTP
            ├── usuario/              # Componente formulario
            └── usuario-consulta/     # Componente grilla
```

---

## 🔧 Backend - API REST

### Características Técnicas

- **.NET 8** con ASP.NET Core Web API
- **Arquitectura por Capas** (Controllers, Services, Domain, Persistence)
- **Entity Framework Core 8.0** (solo para migraciones de BD)
- **ADO.NET** (Microsoft.Data.SqlClient) para acceso a datos
- **Stored Procedure** único (`Usuario_CRUD`) para todas las operaciones
- **Inyección de Dependencias** configurada
- **CORS** habilitado para `http://localhost:4200`
- **Swagger/OpenAPI** para documentación automática
- **Validaciones** en DTOs y capa de servicio

### Estructura de Capas

```
Controllers (Presentación)
    ↓
Application (Lógica de negocio)
    ↓
Domain (Entidades)
    ↓
Outbound/Persistence (Repositorios con ADO.NET)
    ↓
SQL Server (Stored Procedure)
```

### Endpoints de la API

| Método | Endpoint             | Descripción                  |
| ------ | -------------------- | ---------------------------- |
| POST   | `/api/usuarios`      | Agregar nuevo usuario        |
| PUT    | `/api/usuarios/{id}` | Modificar usuario existente  |
| GET    | `/api/usuarios`      | Consultar todos los usuarios |
| GET    | `/api/usuarios/{id}` | Consultar usuario por ID     |
| DELETE | `/api/usuarios/{id}` | Eliminar usuario             |

### Modelo de Datos

```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }          // Máx 100 caracteres
    public DateTime FechaNacimiento { get; set; }
    public char Sexo { get; set; }              // 'M' o 'F'
}
```

### Validaciones del Backend

- **Nombre**: Requerido, máximo 100 caracteres
- **FechaNacimiento**: Requerido, no puede ser futura, edad entre 0-150 años
- **Sexo**: Requerido, solo acepta 'M' o 'F'

### Tecnologías Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8.0 (migraciones)
- ADO.NET (Microsoft.Data.SqlClient)
- SQL Server
- Swagger/OpenAPI

---

## 🎨 Frontend - Angular

### Características Técnicas

- **Angular 20.3.0** con Zoneless Change Detection
- **Tailwind CSS 3.4.17** para estilos modernos
- **Standalone Components** (sin NgModules)
- **Lazy Loading** de componentes
- **Reactive Forms** con validaciones
- **HttpClient** con fetch API
- **Signals** para estado reactivo
- **Control Flow Syntax** (`@if`, `@for`)

### Páginas Implementadas

#### 1. Usuario Consulta (`/consulta`)

- Grilla con todos los usuarios registrados
- Columnas: ID, Nombre, Fecha de Nacimiento, Sexo
- Botones **Modificar** y **Eliminar** por usuario
- Modal de confirmación para eliminar
- Estado vacío cuando no hay usuarios
- Indicadores visuales con badges de color

#### 2. Usuario Formulario (`/usuario`)

- **Campo Nombre**: Input de texto
- **Campo Fecha de Nacimiento**: Input tipo calendario (date)
- **Campo Sexo**: DropDownList (Masculino/Femenino)
- Validaciones en tiempo real
- Mensajes de éxito/error
- Modo creación y edición

### Rutas de la Aplicación

| Ruta           | Componente      | Descripción              |
| -------------- | --------------- | ------------------------ |
| `/`            | (redirect)      | Redirige a `/consulta`   |
| `/consulta`    | UsuarioConsulta | Lista de usuarios        |
| `/usuario`     | Usuario         | Crear nuevo usuario      |
| `/usuario/:id` | Usuario         | Editar usuario existente |

### Características de UI

- Diseño responsivo (mobile-first)
- Gradientes y colores modernos
- Animaciones suaves de transición
- Loading states durante operaciones
- Mensajes de feedback al usuario
- Componentes accesibles (ARIA labels)

### Tecnologías Frontend

- Angular 20.3.0
- TypeScript 5.9.2
- Tailwind CSS 3.4.17
- RxJS 7.8.0
- Angular Router
- Angular Forms (Reactive)
- Angular HttpClient

---

## 🚀 Instalación y Ejecución

### Prerrequisitos

- **Node.js** 18+ y npm
- **.NET SDK** 8.0+
- **SQL Server** (LocalDB, Express o completo)
- **Angular CLI** 20.3.0

### 1. Configurar Base de Datos

#### Actualizar cadena de conexión

Edita `Backend/Business.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=UsuariosDB;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  }
}
```

#### Crear base de datos y tablas

```powershell
cd Backend/Business.Api

# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migración (crea la tabla Usuario)
dotnet ef database update

# Crear migración para Stored Procedure
dotnet ef migrations add AddUsuarioCRUDStoredProcedure
```

Edita el archivo de migración generado y agrega:

**Método `Up`:**

```csharp
migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.Usuario_CRUD
    @Accion NVARCHAR(10),
    @Id INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaNacimiento DATE = NULL,
    @Sexo CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Accion = 'ADD'
    BEGIN
        INSERT INTO Usuario (Nombre, FechaNacimiento, Sexo)
        VALUES (@Nombre, @FechaNacimiento, @Sexo);
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
    END

    IF @Accion = 'UPD'
    BEGIN
        UPDATE Usuario
        SET Nombre = @Nombre,
            FechaNacimiento = @FechaNacimiento,
            Sexo = @Sexo
        WHERE Id = @Id;
    END

    IF @Accion = 'DEL'
    BEGIN
        DELETE FROM Usuario WHERE Id = @Id;
    END

    IF @Accion = 'GET'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario ORDER BY Id;
    END

    IF @Accion = 'GETONE'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario WHERE Id = @Id;
    END
END
");
```

**Método `Down`:**

```csharp
migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.Usuario_CRUD");
```

Luego aplica la migración:

```powershell
dotnet ef database update
```

### 2. Ejecutar Backend

```powershell
cd Backend/Business.Api
dotnet run
```

La API estará disponible en:

- **HTTPS**: `https://localhost:7254`
- **HTTP**: `http://localhost:5000`
- **Swagger**: `https://localhost:7254/swagger`

### 3. Ejecutar Frontend

En otra terminal:

```powershell
cd Frontend

# Instalar dependencias (primera vez)
npm install

# Iniciar servidor de desarrollo
npm start
```

La aplicación estará disponible en:

- **Local**: `http://localhost:4200`

---

## 💻 Uso de la Aplicación

### Flujo de Trabajo

1. **Abrir la aplicación**: `http://localhost:4200`
2. **Ver usuarios**: La página de consulta se carga automáticamente
3. **Crear usuario**:
   - Click en "Nuevo Usuario"
   - Completar formulario
   - Click en "Guardar Usuario"
4. **Editar usuario**:
   - Click en "Modificar" en la grilla
   - Actualizar campos
   - Click en "Actualizar Usuario"
5. **Eliminar usuario**:
   - Click en "Eliminar" en la grilla
   - Confirmar en el modal
   - El usuario se elimina

### Ejemplos de Request

#### Crear Usuario (POST)

```json
{
  "nombre": "Juan Pérez",
  "fechaNacimiento": "1990-05-15",
  "sexo": "M"
}
```

#### Actualizar Usuario (PUT)

```json
{
  "nombre": "Juan Pérez Actualizado",
  "fechaNacimiento": "1990-05-15",
  "sexo": "M"
}
```

### Ejemplos de Response

#### Usuario Individual (GET)

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "fechaNacimiento": "1990-05-15",
  "sexo": "M"
}
```

#### Lista de Usuarios (GET)

```json
[
  {
    "id": 1,
    "nombre": "Juan Pérez",
    "fechaNacimiento": "1990-05-15",
    "sexo": "M"
  },
  {
    "id": 2,
    "nombre": "María García",
    "fechaNacimiento": "1985-08-20",
    "sexo": "F"
  }
]
```

---

## 🧪 Pruebas

### Backend

#### Con Swagger UI

1. Navega a `https://localhost:7254/swagger`
2. Expande los endpoints
3. Click en "Try it out"
4. Completa los datos
5. Click en "Execute"

#### Con curl

```bash
# Obtener todos los usuarios
curl https://localhost:7254/api/usuarios

# Crear usuario
curl -X POST https://localhost:7254/api/usuarios \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Test User","fechaNacimiento":"2000-01-01","sexo":"M"}'

# Actualizar usuario
curl -X PUT https://localhost:7254/api/usuarios/1 \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Updated User","fechaNacimiento":"2000-01-01","sexo":"M"}'

# Eliminar usuario
curl -X DELETE https://localhost:7254/api/usuarios/1
```

### Frontend

```bash
cd Frontend

# Ejecutar pruebas unitarias
npm test

# Ejecutar pruebas con coverage
npm test -- --code-coverage
```

---

## 🔒 Seguridad

### Backend

- Validación de entrada en DTOs
- Stored Procedures para prevenir SQL Injection
- CORS configurado para orígenes específicos
- HTTPS habilitado por defecto

### Frontend

- Validación en formularios
- Sanitización de HTML automática de Angular
- HttpClient con protección CSRF
- Rutas protegidas con guards (si se implementan)

---

## 📚 Tecnologías Utilizadas

### Backend

| Tecnología               | Versión | Propósito                   |
| ------------------------ | ------- | --------------------------- |
| .NET                     | 8.0     | Framework principal         |
| ASP.NET Core             | 8.0     | Web API                     |
| Entity Framework Core    | 8.0     | Migraciones de BD           |
| Microsoft.Data.SqlClient | 5.2+    | ADO.NET para acceso a datos |
| SQL Server               | 2019+   | Base de datos               |
| Swashbuckle.AspNetCore   | 6.5+    | Documentación Swagger       |

### Frontend

| Tecnología         | Versión | Propósito                |
| ------------------ | ------- | ------------------------ |
| Angular            | 20.3.0  | Framework SPA            |
| TypeScript         | 5.9.2   | Lenguaje de programación |
| Tailwind CSS       | 3.4.17  | Framework CSS            |
| RxJS               | 7.8.0   | Programación reactiva    |
| Angular Router     | 20.3.0  | Navegación y rutas       |
| Angular Forms      | 20.3.0  | Formularios reactivos    |
| Angular HttpClient | 20.3.0  | Peticiones HTTP          |

---

## 🐛 Troubleshooting

### Backend

#### Error de conexión a SQL Server

```
Solución:
- Verifica que SQL Server esté ejecutándose
- Confirma la cadena de conexión en appsettings.Development.json
- Verifica los permisos del usuario de la BD
```

#### Error al crear migraciones

```bash
# Solución: Asegúrate de estar en la carpeta correcta
cd Backend/Business.Api
dotnet build
dotnet ef migrations add MigrationName
```

### Frontend

#### Error de CORS

```
Solución:
Verifica que el backend tenga configurado el origen correcto:
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

#### Error con Tailwind CSS

```bash
# Solución: Reinstalar dependencias
cd Frontend
npm uninstall tailwindcss
npm install -D tailwindcss@3.4.17 postcss autoprefixer
```

#### Error de conexión a la API

```
Solución:
- Verifica que el backend esté ejecutándose en https://localhost:7254
- Revisa src/app/services/usuario.service.ts y confirma la URL
- Abre la consola del navegador para más detalles
```

---

## 📞 Contacto y Soporte

Para preguntas, problemas o sugerencias sobre el proyecto:

- Revisa la documentación en los README específicos:
  - [Backend README](Backend/README.md)
  - [Frontend README](Frontend/README.md)
- Verifica los logs de la aplicación
- Consulta la documentación de Swagger en `/swagger`

---

## 📄 Licencia

Este proyecto es un sistema de gestión de usuarios con fines educativos y de demostración.

---

**Desarrollado con .NET 8, Angular 20 y Tailwind CSS** 🚀
