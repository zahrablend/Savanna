using Common.Interfaces;
using Common.ValueObjects;

namespace CodeLibrary.GameEngine;

public class GameSetup
{
    private readonly IGameField _gameField;
    private readonly Random _random = new();
    private readonly List<IAnimal> _animals = new();
    private readonly AnimalDictionary _animalDict;
    private Dictionary<string, IAnimalFactory> _animalFactories;

    public GameSetup(IGameField gameField, AnimalDictionary animalDict)
    {
        _gameField = gameField;
        _animalDict = animalDict;
        _animalFactories = new Dictionary<string, IAnimalFactory>();
    }

    public IAnimalFactory GetAnimalFactoryBySpecies(string species)
    {
        if (_animalFactories.ContainsKey(species))
        {
            return _animalFactories[species];
        }
        else
        {
            throw new Exception($"No factory found for species {species}");
        }
    }

    public void AddAnimalFactory(IAnimalFactory animalFactory)
    {
        _animalFactories[animalFactory.Species] = animalFactory;
    }

    public int GetAnimalCount(string species)
    {
        return _animals.Count(animal => animal.Species == species);
    }

    public IAnimal AddAnimal(IAnimalFactory animalFactory)
    {
        _animalDict.RepresentAnimal(animalFactory);
        var animal = animalFactory.Create();
        int x, y;
        do
        {
            x = _random.Next(_gameField.Width);
            y = _random.Next(_gameField.Height);
        } while (!_gameField.GetState(x, y).IsEmpty);

        animal.X = x;
        animal.Y = y;
        _gameField.SetState(x, y, new FieldCell { State = animal });
        _animals.Add(animal);
        return animal;
    }

    public List<IAnimal> GetAnimals()
    {
        return _animals;
    }

    public string DisplayAnimalInfo(IAnimal animal)
    {
        return $"Species: {animal.Species}, {Environment.NewLine}" +
            $"Health: {animal.Health}, {Environment.NewLine}" +
            $"Age: {animal.Age},{Environment.NewLine} " +
            $"Offspring: {animal.Offspring}";
    }

    public enum DisplayType
    {
        Symbol,
        Icon
    }

    //public string DisplayAnimalRepresentation(IAnimal gameAnimal, DisplayType displayType)
    //{
    //    if (_animalDict.TryGetRepresentation(gameAnimal.Species, out AnimalRepresentation representation))
    //    {
    //        return displayType == DisplayType.Symbol ? $"{representation.Symbol}" : $"{representation.Icon}";
    //    }
    //    else
    //    {
    //        throw new KeyNotFoundException($"No representation found for species {gameAnimal.Species}");
    //    }
    //}
    public string DisplayAnimalRepresentation(IAnimal gameAnimal, DisplayType displayType)
    {
        if (_animalDict.TryGetRepresentation(gameAnimal.Species, out AnimalRepresentation representation))
        {
            return displayType == DisplayType.Symbol ? $"{representation.Symbol}" : $"{representation.Icon}";
        }
        else
        {
            throw new KeyNotFoundException($"No representation found for species {gameAnimal.Species}");
        }
    }


    public IGameField GameField => _gameField;
}
