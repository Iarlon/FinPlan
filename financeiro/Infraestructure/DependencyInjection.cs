using financeiro.Application.Contract;
using financeiro.Infraestructure.Database;
using financeiro.Infraestructure.Events;
using Financeiro.Infraestructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Financeiro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

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
        var key = configuration["Jwt:Key"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key!))
                };
            });

        return services;
    }
}
