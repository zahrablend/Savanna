using Common.Interfaces;

namespace AntelopeBehaviour;

public class AntelopeFactory : IAnimalFactory
{
    public char Symbol => new Antelope().Symbol;

    public IAnimal Create()
    {
        return new Antelope();
    }
}
