using Common.Interfaces;

namespace Savanna.Infrastructure;

public class AnimalFactoryLoader
{
    public IAnimalFactory LoadAnimalFactory(string assemblyPath, string typeName)
    {
        var assembly = AssemblyLoader.LoadAssembly(assemblyPath);
        if (assembly == null)
        {
            throw new InvalidOperationException("Failed to load assembly at path: " + assemblyPath);
        }

        var factoryType = assembly.GetType(typeName);

        if (factoryType == null)
        {
            throw new TypeLoadException("Type '" + typeName + "' not found in assembly.");
        }

        return (IAnimalFactory)Activator.CreateInstance(factoryType);
    }
}

