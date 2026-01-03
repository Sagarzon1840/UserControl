-- Script para crear el Stored Procedure Usuario_CRUD
-- Este script se ejecutará desde una migración de EF Core
-- pero se proporciona aquí como referencia

USE UsuariosDB;
GO

-- Eliminar el procedimiento si existe
IF OBJECT_ID('dbo.Usuario_CRUD', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Usuario_CRUD;
GO

-- Crear el procedimiento
CREATE PROCEDURE dbo.Usuario_CRUD
    @Accion NVARCHAR(10),
    @Id INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaNacimiento DATE = NULL,
    @Sexo CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Agregar nuevo usuario
    IF @Accion = 'ADD'
    BEGIN
        INSERT INTO Usuario (Nombre, FechaNacimiento, Sexo)
        VALUES (@Nombre, @FechaNacimiento, @Sexo);
        
        -- Devolver el ID del nuevo registro
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
    END
    
    -- Actualizar usuario existente
    IF @Accion = 'UPD'
    BEGIN
        UPDATE Usuario
        SET Nombre = @Nombre,
            FechaNacimiento = @FechaNacimiento,
            Sexo = @Sexo
        WHERE Id = @Id;
    END
    
    -- Eliminar usuario
    IF @Accion = 'DEL'
    BEGIN
        DELETE FROM Usuario
        WHERE Id = @Id;
    END
    
    -- Consultar todos los usuarios
    IF @Accion = 'GET'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario
        ORDER BY Id;
    END
    
    -- Consultar un usuario por ID
    IF @Accion = 'GETONE'
    BEGIN
        SELECT Id, Nombre, FechaNacimiento, Sexo
        FROM Usuario
        WHERE Id = @Id;
    END
END
GO

-- Ejemplos de uso:

-- Agregar usuario
-- EXEC dbo.Usuario_CRUD @Accion = 'ADD', @Nombre = 'Juan Pérez', @FechaNacimiento = '1990-05-15', @Sexo = 'M';

-- Consultar todos
-- EXEC dbo.Usuario_CRUD @Accion = 'GET';

-- Consultar por ID
-- EXEC dbo.Usuario_CRUD @Accion = 'GETONE', @Id = 1;

-- Actualizar
-- EXEC dbo.Usuario_CRUD @Accion = 'UPD', @Id = 1, @Nombre = 'Juan Carlos Pérez', @FechaNacimiento = '1990-05-15', @Sexo = 'M';

-- Eliminar
-- EXEC dbo.Usuario_CRUD @Accion = 'DEL', @Id = 1;
