using Data.Contracts.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Business.Api.Outbound.Persistence;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") 
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
    }

    public async Task<int> AgregarAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("dbo.Usuario_CRUD", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Accion", "ADD");
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
        command.Parameters.AddWithValue("@Sexo", usuario.Sexo);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> ModificarAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("dbo.Usuario_CRUD", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Accion", "UPD");
        command.Parameters.AddWithValue("@Id", usuario.Id);
        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
        command.Parameters.AddWithValue("@Sexo", usuario.Sexo);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        
        if (result == null || result == DBNull.Value)
        {
            return false;
        }
        
        return Convert.ToInt32(result) > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("dbo.Usuario_CRUD", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Accion", "DEL");
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        
        if (result == null || result == DBNull.Value)
        {
            return false;
        }
        
        return Convert.ToInt32(result) > 0;
    }

    public async Task<IEnumerable<Usuario>> ConsultarAsync()
    {
        var usuarios = new List<Usuario>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("dbo.Usuario_CRUD", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Accion", "GET");

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            usuarios.Add(MapUsuario(reader));
        }

        return usuarios;
    }

    public async Task<Usuario?> ConsultarPorIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("dbo.Usuario_CRUD", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Accion", "GETONE");
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapUsuario(reader);
        }

        return null;
    }

    private static Usuario MapUsuario(SqlDataReader reader)
    {
        return new Usuario
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
            Sexo = reader.GetString(reader.GetOrdinal("Sexo"))[0]
        };
    }
}
