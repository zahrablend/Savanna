using Common.Interfaces;
using Common.ValueObjects;

namespace CodeLibrary.GameEngine;

public class AnimalRemover
{ 
    private IGameField _gameField;

    public AnimalRemover(IGameField gameField)
    {
        _gameField = gameField;
    }

    public void RemoveAnimalOnDeath(IAnimal animal)
    {
        if (animal.Health <= 0)
        {
            _gameField.SetState(animal.X, animal.Y, new FieldCell { State = null });
        }
    }
}
