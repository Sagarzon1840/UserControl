using Business.Api.Application.DTOs;
using Data.Contracts.Entities;
using Business.Api.Outbound.Persistence;

namespace Business.Api.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<int> AgregarUsuarioAsync(UsuarioRequestDto request)
    {
        ValidarEdad(request.FechaNacimiento);

        var usuario = new Usuario
        {
            Nombre = request.Nombre.Trim(),
            FechaNacimiento = request.FechaNacimiento.Date,
            Sexo = request.Sexo
        };

        var id = await _usuarioRepository.AgregarAsync(usuario);
        _logger.LogInformation("Usuario creado con ID: {Id}", id);
        return id;
    }

    public async Task<bool> ModificarUsuarioAsync(int id, UsuarioRequestDto request)
    {
        ValidarEdad(request.FechaNacimiento);

        var usuarioExistente = await _usuarioRepository.ConsultarPorIdAsync(id);
        if (usuarioExistente == null)
        {
            _logger.LogWarning("Usuario con ID {Id} no encontrado", id);
            return false;
        }

        var usuario = new Usuario
        {
            Id = id,
            Nombre = request.Nombre.Trim(),
            FechaNacimiento = request.FechaNacimiento.Date,
            Sexo = request.Sexo
        };

        var resultado = await _usuarioRepository.ModificarAsync(usuario);
        _logger.LogInformation("Usuario con ID {Id} modificado: {Resultado}", id, resultado);
        return resultado;
    }

    public async Task<bool> EliminarUsuarioAsync(int id)
    {
        var usuarioExistente = await _usuarioRepository.ConsultarPorIdAsync(id);
        if (usuarioExistente == null)
        {
            _logger.LogWarning("Usuario con ID {Id} no encontrado para eliminar", id);
            return false;
        }

        var resultado = await _usuarioRepository.EliminarAsync(id);
        _logger.LogInformation("Usuario con ID {Id} eliminado: {Resultado}", id, resultado);
        return resultado;
    }

    public async Task<IEnumerable<UsuarioResponseDto>> ConsultarUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.ConsultarAsync();
        return usuarios.Select(u => new UsuarioResponseDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            FechaNacimiento = u.FechaNacimiento,
            Sexo = u.Sexo
        });
    }

    public async Task<UsuarioResponseDto?> ConsultarUsuarioPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.ConsultarPorIdAsync(id);
        if (usuario == null)
        {
            return null;
        }

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            FechaNacimiento = usuario.FechaNacimiento,
            Sexo = usuario.Sexo
        };
    }

    private void ValidarEdad(DateTime fechaNacimiento)
    {
        if (fechaNacimiento > DateTime.Today)
        {
            throw new ArgumentException("La fecha de nacimiento no puede ser futura");
        }

        var edad = DateTime.Today.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
        {
            edad--;
        }

        if (edad < 0 || edad > 150)
        {
            throw new ArgumentException("La edad debe estar entre 0 y 150 años");
        }
    }
}
