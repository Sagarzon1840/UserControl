using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Contracts.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioCRUDStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar el SP si existe
            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.Usuario_CRUD', 'P') IS NOT NULL
    DROP PROCEDURE dbo.Usuario_CRUD;
");

            // Crear el SP
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
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.Usuario_CRUD");
        }
    }
}
