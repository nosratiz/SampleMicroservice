using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace Frontliners.Common.InfraStructure.Helper;

public static class AssemblyHelper
{
    private static List<Assembly>? _assemblies;
    private static List<Type>? _types;

    private static Func<string> _resolveBaseDirectory = () => AppDomain.CurrentDomain.BaseDirectory;

    private static Func<string, Assembly> _loadAssembly = dll =>
        AssemblyLoadContext.Default.LoadFromAssemblyName(
            new AssemblyName(Path.GetFileNameWithoutExtension(dll)));

    public static void SetLoadAssembly(Func<string, Assembly> loadAssembly) =>
        _loadAssembly = loadAssembly;

    public static void SetResolveBaseDirectory(Func<string> resolveBaseDirectory) =>
        _resolveBaseDirectory = resolveBaseDirectory;

    public static Assembly Find(string partOfName)
    {
        LoadAllBinDirectoryAssemblies();

        if (_assemblies?.Any() != true) throw new Exception("Assemblies list is empty");

        var foundAssembly = _assemblies.SingleOrDefault(a => a.GetName().Name?.Contains(partOfName) == true);

        return foundAssembly ?? throw new Exception($"Assembly not found with part of name '{partOfName}'");
    }

    public static Assembly GetByName(string name)
    {
        LoadAllBinDirectoryAssemblies();

        if (_assemblies?.Any() != true) throw new Exception("Assemblies list is empty");

        var foundAssembly = _assemblies.SingleOrDefault(a => a.GetName().Name == name);

        return foundAssembly ?? throw new Exception($"Assembly not found with exact name '{name}'");
    }

    private static void LoadAllBinDirectoryAssemblies()
    {
        if (_assemblies?.Any() == true) return;

        _assemblies = new List<Assembly>();

        var binPath = _resolveBaseDirectory();

        var files = Directory.GetFiles(binPath, "*.dll")
            .Where(a =>
                Path.GetFileNameWithoutExtension(a).Contains("Frontliners"))
                .ToList();

        foreach (var dll in files)
        {
            try
            {
                _assemblies.Add(_loadAssembly(dll));
            }
            catch (FileLoadException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (BadImageFormatException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }

    private static List<Assembly> FindMyAssemblies()
    {
        LoadAllBinDirectoryAssemblies();
        return _assemblies ?? throw new Exception("Assemblies list is empty");
    }

    public static List<Type> FindAllTypes()
    {
        if (_types?.Any() == true) return _types;

        _types = FindMyAssemblies().SelectMany(a => a.GetTypes()).ToList();

        return _types ?? throw new Exception("Types list is empty");
    }

    public static string GetVersion(this Assembly? assembly)
    {
        if (assembly == null) return "Not found";
        var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        return
            $"{assembly.GetName().Version?.ToString() ?? "Assembly version not found"} - {fvi.FileVersion ?? "File version not found"} - {fvi.ProductVersion ?? "Product version not found"}";
    }
}