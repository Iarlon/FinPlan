using financeiro.Domain.Common;
using financeiro.Infraestructure.Database;
using Financeiro.Infraestructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(c => c
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && (t.Name.EndsWith("Repository") || t.Name.EndsWith("Service"))
                )
            )
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
