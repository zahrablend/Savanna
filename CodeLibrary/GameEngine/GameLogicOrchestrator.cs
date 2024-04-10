using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class GameLogicOrchestrator
{
    private IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private AnimalMover _animalMover;
    private HealthMetricCounter _healthMetricCounter;
    private AnimalRemover _animalRemover;
    private AnimalCreator _animalCreator;

    public GameLogicOrchestrator() { }

    public GameLogicOrchestrator(FieldDisplayer fieldDisplayer, IGameFieldFactory gameFieldFactory, GameSetup gameSetup)
    {
        _fieldDisplayer = fieldDisplayer;
        _gameField = gameFieldFactory.Create(_fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
        
        _animalMover = new AnimalMover(_gameField, fieldDisplayer);
        _healthMetricCounter = new HealthMetricCounter(_gameField, fieldDisplayer);
        _animalRemover = new AnimalRemover(_gameField, gameSetup);
        _animalCreator = new AnimalCreator(gameSetup);
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
    public IGameField GetGameField => _gameField;
}
