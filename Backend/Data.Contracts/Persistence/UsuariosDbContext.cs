using Microsoft.EntityFrameworkCore;
using Data.Contracts.Entities;

namespace Data.Contracts.Persistence;

public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            // Configurar tabla con check constraint (EF Core 8 syntax)
            entity.ToTable("Usuario", t =>
            {
                t.HasCheckConstraint("CK_Usuario_Sexo", "Sexo IN ('M', 'F')");
            });

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            entity.Property(e => e.FechaNacimiento)
                .IsRequired()
                .HasColumnType("date");

            entity.Property(e => e.Sexo)
                .IsRequired()
                .HasColumnType("char(1)");
        });
    }
}