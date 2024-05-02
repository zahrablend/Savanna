using Common.Interfaces;

namespace Common.ValueObjects;

public class AnimalDictionary
{
    private Dictionary<string, AnimalRepresentation> _animalDict;

    public AnimalDictionary()
    {
        _animalDict = new Dictionary<string, AnimalRepresentation>();
    }

    //Added for debugging
    public IEnumerable<KeyValuePair<string, AnimalRepresentation>> GetRepresentations()
    {
        return _animalDict;
    }

    public void RepresentAnimal(IAnimalFactory animalFactory)
    {
        _animalDict[animalFactory.Species] = new AnimalRepresentation { Symbol = animalFactory.Symbol, Icon = animalFactory.Icon };
    }

    public bool TryGetRepresentation(string species, out AnimalRepresentation representation)
    {
        return _animalDict.TryGetValue(species, out representation);
    }
}

