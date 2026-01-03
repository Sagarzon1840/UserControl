using Business.Api.Application.Services;
using Business.Api.Outbound.Persistence;

namespace Business.Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
        return services;
    }
}
