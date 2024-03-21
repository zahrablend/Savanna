using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary.Factories;

public class AntelopeFactory : IAnimalFactory
{
    public char Symbol => new Antelope().Symbol;

    public IAnimal Create()
    {
        return new Antelope();
    }
}
