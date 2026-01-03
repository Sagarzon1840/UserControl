using Business.Api.Application.DTOs;

namespace Business.Api.Application.Services;

public interface IUsuarioService
{
    Task<int> AgregarUsuarioAsync(UsuarioRequestDto request);
    Task<bool> ModificarUsuarioAsync(int id, UsuarioRequestDto request);
    Task<bool> EliminarUsuarioAsync(int id);
    Task<IEnumerable<UsuarioResponseDto>> ConsultarUsuariosAsync();
    Task<UsuarioResponseDto?> ConsultarUsuarioPorIdAsync(int id);
}
