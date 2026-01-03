using Data.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Contracts;

public static class DependencyInjection
{
    public static IServiceCollection AddDataContracts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsuariosDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly("Data.Contracts")));

        return services;
    }
}
