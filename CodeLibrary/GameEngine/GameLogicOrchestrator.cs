using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class GameLogicOrchestrator
{
    private AnimalMover _animalMover;
    private HealthMetricCounter _healthMetricCounter;
    private AnimalRemover _animalRemover;

    public GameLogicOrchestrator() { }

    public GameLogicOrchestrator(IGameField gameField, FieldDisplayer fieldDisplayer, GameSetup gameSetup)
    {
        _animalMover = new AnimalMover(gameField, fieldDisplayer);
        _healthMetricCounter = new HealthMetricCounter(gameField, fieldDisplayer);
        _animalRemover = new AnimalRemover(gameField);
    }

    public void PlayGame(IAnimal animal)
    {
        _animalMover.MoveAnimal(animal);
        _healthMetricCounter.DecreaseHealth(animal);
        _healthMetricCounter.InteractWith(animal);
        _animalRemover.RemoveAnimalOnDeath(animal);
    }
}
