-- Script para crear la tabla Usuario
-- Ejecutar ANTES de crear el Stored Procedure

USE [MiBaseDeDatos];
GO

-- Eliminar tabla si existe (solo para desarrollo)
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL
    DROP TABLE dbo.Usuario;
GO

-- Crear la tabla Usuario
CREATE TABLE dbo.Usuario (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL,
    CONSTRAINT PK_Usuario PRIMARY KEY (Id),
    CONSTRAINT CK_Usuario_Sexo CHECK (Sexo IN ('M', 'F'))
);
GO

-- Verificar que se creó correctamente
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuario'
ORDER BY ORDINAL_POSITION;
GO

-- Ver los constraints
SELECT 
    tc.CONSTRAINT_NAME,
    tc.CONSTRAINT_TYPE
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
WHERE tc.TABLE_NAME = 'Usuario';
GO

PRINT 'Tabla Usuario creada exitosamente';
