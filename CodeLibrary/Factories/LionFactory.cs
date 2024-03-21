using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary.Factories;

public class LionFactory : IAnimalFactory
{
    public char Symbol => new Lion().Symbol;

    public IAnimal Create()
    {
        return new Lion();
    }
}
