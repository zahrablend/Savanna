using CodeLibrary.GameEngine;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class GameLogic
{
    private IAnimal?[,] _gameField; 
    private List<IAnimal> _animals; 
    private readonly FieldDisplayer.FieldSize _fieldSize;
    private readonly FieldDisplayer _fieldDisplayer;
    private AnimalMover _animalMover;
    private HealthMetricCounter _healthMetricCounter;
    private AnimalRemover _animalRemover;
    public GameLogic(FieldDisplayer.FieldSize fieldSize, FieldDisplayer fieldDisplayer)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = new List<IAnimal>();
        _fieldDisplayer = fieldDisplayer;
        _animalMover = new AnimalMover(_gameField, _fieldSize);
        _healthMetricCounter = new HealthMetricCounter(_gameField, _fieldSize);
        _animalRemover = new AnimalRemover(_gameField, _animals);
    }

    public GameLogic(FieldDisplayer.FieldSize fieldSize, FieldDisplayer fieldDisplayer, AnimalMover animalMover, HealthMetricCounter healthMetricCounter, AnimalRemover animalRemover)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = new List<IAnimal>();
        _fieldDisplayer = fieldDisplayer;
        _animalMover = animalMover;
        _healthMetricCounter = healthMetricCounter;
        _animalRemover = animalRemover;
    }

    public void PlayGame(IAnimal animal)
    {
        _animalMover.MoveAnimal(animal);
        _healthMetricCounter.DecreaseHealth(animal);
        _healthMetricCounter.InteractWith(animal);
        _animalRemover.RemoveAnimalOnDeath(animal);
    }

    public string DrawField => _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    public List<IAnimal> GetAnimals => _animals;
    public IAnimal[,] GetGameField => _gameField;
}
