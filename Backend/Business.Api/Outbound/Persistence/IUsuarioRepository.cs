using Data.Contracts.Entities;

namespace Business.Api.Outbound.Persistence;

public interface IUsuarioRepository
{
    Task<int> AgregarAsync(Usuario usuario);
    Task<bool> ModificarAsync(Usuario usuario);
    Task<bool> EliminarAsync(int id);
    Task<IEnumerable<Usuario>> ConsultarAsync();
    Task<Usuario?> ConsultarPorIdAsync(int id);
}
