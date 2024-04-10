using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalRemover
{ 
    private IGameField _gameField;
    private GameSetup _gameSetup;

    public AnimalRemover(IGameField gameField, GameSetup gameSetup)
    {
        _gameField = gameField;
        _gameSetup = gameSetup;
    }

    /// <summary>
    /// If animals health is zero or less, removes the specified animal from the game field and animals list.
    /// </summary>
    /// <param name="animal"></param>
    public void RemoveAnimalOnDeath(IAnimal animal)
    {
        if (animal.Health <= 0)
        {
            _gameField.SetState(animal.X, animal.Y, null);
            _gameSetup.GetAnimals.Remove(animal);
        }
    }
}
