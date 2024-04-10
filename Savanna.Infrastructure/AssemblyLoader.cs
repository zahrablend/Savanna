using System.Reflection;

namespace Savanna.Infrastructure;

public static class AssemblyLoader
{
    public static Assembly? LoadAssembly(string path)
    {
        try
        {
            var assembly = Assembly.LoadFile(path);
            var assemblyName = Path.GetFileNameWithoutExtension(path);
            if (assembly.GetName().Name != assemblyName)
            {
                throw new ArgumentException("Invalid assembly name.");
            }

            return assembly;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.FileName}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}
