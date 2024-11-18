using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frontliners.Common.InfraStructure.Helper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllScoped<TInterface>(this IServiceCollection services, List<Type> allTypes)
    {
        return services.AddAll<TInterface>(allTypes, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddAllTransient<TInterface>(this IServiceCollection services, List<Type> allTypes)
    {
        return services.AddAll<TInterface>(allTypes, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddAllSingleton<TInterface>(this IServiceCollection services, List<Type> allTypes)
    {
        return services.AddAll<TInterface>(allTypes, ServiceLifetime.Singleton);
    }

    private static IServiceCollection AddAll<TInterface>(this IServiceCollection services, List<Type> allTypes, ServiceLifetime lifetime)
    {
        var interfaceType = typeof(TInterface);

        var types = allTypes.GetInterfacesBy(interfaceType);

        foreach (var type in types)
        {
            var classType = allTypes.GetClassTypeFromInterface(type);
            services.Add(new ServiceDescriptor(type, classType, lifetime));
            services.Add(new ServiceDescriptor(interfaceType, classType, lifetime));
        }

        return services;
    }

    public static TSettings? AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration)
        where TSettings : class, new()
    {
        return services.AddConfig<TSettings>(configuration, _ => { });
    }

    private static TSettings? AddConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, Action<BinderOptions> configureOptions)
        where TSettings : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);

        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<TSettings>(configuration, configureOptions);

        var setting = configuration.Get<TSettings>(configureOptions);

        return setting;
    }
}