using Common.Interfaces;

namespace Common.ValueObjects;

public class AnimalDictionary
{
    private Dictionary<string, (char Symbol, string Icon)> _animalDict;

    public AnimalDictionary()
    {
        _animalDict = new Dictionary<string, (char, string)>();
    }

    public void RepresentAnimal(IAnimalFactory animalFactory)
    {
        _animalDict[animalFactory.Species] = (animalFactory.Symbol, animalFactory.Icon);
    }

    public (char Symbol, string Icon) this[string species]
    {
        get { return _animalDict[species]; }
    }
}

