# Sistema de Gestión de Usuarios - Arquitectura por Capas

## Arquitectura del Proyecto

### Estructura de Capas

```
Business.Api/
??? Controllers/              # Endpoints REST (Presentación)
??? Application/             # Lógica de negocio
?   ??? Services/           # Servicios de aplicación
?   ??? DTOs/              # Data Transfer Objects
?   ??? DependencyInjection.cs
??? Domain/                 # Entidades del dominio
?   ??? Entities/
??? Outbound/              # Adaptadores de salida
?   ??? Persistence/       # Repositorios con ADO.NET
??? Data/                  # DbContext y migraciones (solo estructura)
?   ??? UsuariosDbContext.cs
?   ??? DependencyInjection.cs
?   ??? Migrations/        # Se generarán aquí
??? Filters/              # Filtros de validación
```

## Configuración Inicial

### 1. Configurar Base de Datos

Actualiza la cadena de conexión en `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "Default": "Server=localhost,1433;Database=UsuariosDB;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
}
```

### 2. Crear Migración Inicial

Ejecuta en la terminal desde la carpeta del proyecto:

```powershell
# Navegar al proyecto Business.Api
cd Business.Api

# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migración (esto creará la tabla Usuario)
dotnet ef database update
```

### 3. Crear el Stored Procedure

Después de aplicar la migración, necesitas crear una migración adicional para el Stored Procedure:

```powershell
dotnet ef migrations add AddUsuarioCRUDStoredProcedure
```

Luego edita el archivo de migración generado y agrega el siguiente código en el método `Up`:

````csharp
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
        DELETE FROM Usuario
        WHERE Id = @Id;
    END

    IF @Accion = 'GET'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario
        ORDER BY Id;
    END

    IF @Accion = 'GETONE'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario
        WHERE Id = @Id;
    END
END
");

Y en el método `Down`:

```csharp
migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.Usuario_CRUD");
````

Luego aplica la migración:

```powershell
dotnet ef database update
```

## Ejecutar el Proyecto

### Modo Desarrollo Local

```powershell
cd Business.Api
dotnet run
```

La API estará disponible en:

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## Endpoints de la API

### Usuarios

| Método | Endpoint             | Descripción                  |
| ------ | -------------------- | ---------------------------- |
| POST   | `/api/usuarios`      | Agregar nuevo usuario        |
| PUT    | `/api/usuarios/{id}` | Modificar usuario existente  |
| GET    | `/api/usuarios`      | Consultar todos los usuarios |
| GET    | `/api/usuarios/{id}` | Consultar usuario por ID     |
| DELETE | `/api/usuarios/{id}` | Eliminar usuario             |

### Ejemplo de Request (POST/PUT)

```json
{
  "nombre": "Juan Pérez",
  "fechaNacimiento": "1990-05-15",
  "sexo": "M"
}
```

### Ejemplo de Response (GET)

```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "fechaNacimiento": "1990-05-15",
  "sexo": "M"
}
```

## Validaciones Implementadas

- **Nombre**: Requerido, máximo 100 caracteres
- **FechaNacimiento**: Requerido, no puede ser futura, edad entre 0-150 años
- **Sexo**: Requerido, solo acepta 'M' o 'F'

## Características Técnicas

### Cumplimiento de Requerimientos

1. **Arquitectura por Capas**: Separación clara entre Controllers, Services, Domain y Persistence
2. **EF Core solo para estructura**: DbContext usado únicamente para migraciones
3. **Stored Procedure para CRUD**: Todo el acceso a datos usa `Usuario_CRUD`
4. **ADO.NET en Repositorios**: Implementación directa con SqlConnection y SqlCommand
5. **Inyección de Dependencias**: Configurada en Program.cs y extensiones
6. **CORS**: Configurado para `http://localhost:4200` (Angular)
7. **Validaciones**: En DTOs y capa de servicio
8. **Swagger**: Documentación automática de la API

### Tecnologías Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8.0 (solo migraciones)
- ADO.NET (Microsoft.Data.SqlClient)
- SQL Server
- Swagger/OpenAPI

## Pruebas con Swagger

1. Ejecuta el proyecto
2. Navega a `https://localhost:7254/swagger`
3. Prueba cada endpoint directamente desde la interfaz

## Conexión desde Angular

Configura tu servicio Angular para apuntar a:

```typescript
private apiUrl = 'https://localhost:7254/api/usuarios';
```

## Troubleshooting

### Error de conexión a SQL Server

Verifica que:

- SQL Server está ejecutándose
- La cadena de conexión sea correcta
- El usuario `app_user` tenga los permisos necesarios

### Error al crear migraciones

Asegúrate de estar en la carpeta `Business.Api` y que el proyecto compile correctamente:

```powershell
dotnet build
dotnet ef migrations add MigrationName
```
