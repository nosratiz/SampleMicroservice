using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frontliners.Common.InfraStructure.Helper;

public static class TypeExtensions
{
    public static object? DefaultForType(this Type targetType)
    {
        return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
    }
    public static Type GetClassTypeFromInterface(this List<Type> types, Type interfaceType)
    {
        return types.SingleOrDefault(a =>
            a.IsClass &&
            interfaceType.IsAssignableFrom(a) &&
            "I" + a.Name == interfaceType.Name) ?? throw new Exception($"Class type from {interfaceType.Name} not found");
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    private static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }


    public static List<Type> GetInterfacesBy<T>(this List<Type> types)
    {
        var type = typeof(T);
        return types.GetInterfacesBy(type);
    }

    public static List<Type> GetInterfacesBy(this List<Type> types, Type type)
    {
        return types.Where(a => a.IsInterface && type.IsAssignableFrom(a) && a != type && !a.IsGenericType).ToList();
    }

    public static List<Type> GetClassesBy<T>(this List<Type> types)
    {
        var type = typeof(T);
        return types.GetClassesBy(type);
    }

    private static List<Type> GetClassesBy(this List<Type> types, Type type)
    {
        return types.Where(a => a.IsClass && type.IsAssignableFrom(a) && a != type && !a.IsGenericType).ToList();
    }

    public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
    {
        return FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();
    }

    private static IEnumerable<Type> FindInterfacesThatClosesCore(Type? pluggedType, Type templateType)
    {
        if (pluggedType == null) yield break;

        if (!pluggedType.IsConcrete()) yield break;

        if (templateType.GetTypeInfo().IsInterface)
        {
            foreach (
                var interfaceType in
                pluggedType.GetInterfaces()
                    .Where(type => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == templateType))
            {
                yield return interfaceType;
            }
        }
        else if ((pluggedType.GetTypeInfo().BaseType?.GetTypeInfo().IsGenericType ?? false) &&
                 pluggedType.GetTypeInfo().BaseType?.GetGenericTypeDefinition() == templateType)
        {
            yield return pluggedType.GetTypeInfo().BaseType!;
        }

        if (pluggedType.GetTypeInfo().BaseType == typeof(object)) yield break;

        foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
        {
            yield return interfaceType;
        }
    }

    private static bool IsConcrete(this Type type)
    {
        return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
    }

}