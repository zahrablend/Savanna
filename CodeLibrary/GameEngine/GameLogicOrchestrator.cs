using CodeLibrary.Constants;
using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public class GameLogicOrchestrator
{
    private IAnimal?[,] _gameField;
    private List<IAnimal> _animals;
    private readonly FieldDisplayer _fieldDisplayer;
    private readonly IGameUI _gameUI;
    private AnimalMover _animalMover;
    private HealthMetricCounter _healthMetricCounter;
    private AnimalRemover _animalRemover;
    private AnimalCreator _animalCreator;

    public GameLogicOrchestrator() { }

    public GameLogicOrchestrator(FieldDisplayer fieldDisplayer, IGameUI gameUI)
    {
        _fieldDisplayer = fieldDisplayer;
        _gameUI = gameUI;
        _gameField = new IAnimal[_fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width];
        _animals = new List<IAnimal>();

        // Initialize the objects
        _animalMover = new AnimalMover(_gameField, fieldDisplayer.Size);
        _healthMetricCounter = new HealthMetricCounter(_gameField, fieldDisplayer.Size);
        _animalRemover = new AnimalRemover(_gameField, _animals);
        _animalCreator = new AnimalCreator(this);
    }

    public virtual void AddAnimal(IAnimal animal)
    {
        int x;
        int y;

        do
        {
            x = new Random().Next(_fieldDisplayer.Size.Height);
            y = new Random().Next(_fieldDisplayer.Size.Width);
        }
        while (_gameField[x, y] != null);

        animal.X = x;
        animal.Y = y;
        animal.Health = Constant.InitialHealth;
        _gameField[x, y] = animal;
        _animals.Add(animal);
    }

    public void PlayGame(IAnimal animal)
    {
        _animalMover.MoveAnimal(animal);
        _healthMetricCounter.DecreaseHealth(animal);
        _healthMetricCounter.InteractWith(animal);
        _animalRemover.RemoveAnimalOnDeath(animal);
        _animalCreator.CreateAnimalOnBirth();
    }

    public string DrawField => _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
    public List<IAnimal> GetAnimals => _animals;
    public IAnimal[,] GetGameField => _gameField;
}
