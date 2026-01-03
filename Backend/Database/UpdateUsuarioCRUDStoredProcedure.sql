-- Script para actualizar el Stored Procedure Usuario_CRUD
-- Ejecutar este script en la base de datos para corregir el problema de @@ROWCOUNT

USE [MiBaseDeDatos];
GO

-- Eliminar el SP si existe
IF OBJECT_ID('dbo.Usuario_CRUD', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Usuario_CRUD;
GO

-- Crear el SP con la corrección para devolver @@ROWCOUNT correctamente
CREATE PROCEDURE dbo.Usuario_CRUD
    @Accion NVARCHAR(10),
    @Id INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaNacimiento DATE = NULL,
    @Sexo CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RowsAffected INT;

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
        
        SET @RowsAffected = @@ROWCOUNT;
        SELECT @RowsAffected AS RowsAffected;
    END
    
    IF @Accion = 'DEL'
    BEGIN
        DELETE FROM Usuario
        WHERE Id = @Id;
        
        SET @RowsAffected = @@ROWCOUNT;
        SELECT @RowsAffected AS RowsAffected;
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
GO

PRINT 'Stored Procedure Usuario_CRUD actualizado exitosamente';
GO
