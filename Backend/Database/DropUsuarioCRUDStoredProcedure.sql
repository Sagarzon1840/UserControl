-- Script para eliminar el Stored Procedure Usuario_CRUD
-- Ejecutar antes de aplicar la migración

USE [MiBaseDeDatos];
GO

IF OBJECT_ID('dbo.Usuario_CRUD', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.Usuario_CRUD;
    PRINT 'Stored Procedure Usuario_CRUD eliminado exitosamente';
END
ELSE
BEGIN
    PRINT 'Stored Procedure Usuario_CRUD no existe';
END
GO
