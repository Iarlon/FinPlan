namespace Financeiro.Application;

using MediatR;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Action<MediatRServiceConfiguration> configuration = cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
                cfg.TypeEvaluator = type => !IsRecord(type);
            };
        services.AddMediatR(configuration);

        return services;
    }

    private static bool IsRecord(Type type)
    {
        // Records have a compiler-generated <Clone>$ method
        return type.GetMethod("<Clone>$",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance) != null;
    }
}
