using CodeLibrary.Constants;
using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalAdder
{
    private IAnimal?[,] _gameField;
    private List<IAnimal> _animals;
    private readonly FieldDisplayer.FieldSize _fieldSize;

    public AnimalAdder(IAnimal?[,] gameField, FieldDisplayer.FieldSize fieldSize)
    {
        _gameField = gameField;
        _fieldSize = fieldSize;
        _animals = new List<IAnimal>();
    }

    /// <summary>
    /// Adds an animal to the game field at a random unoccupied position.
    /// </summary>
    /// <param name="animal">The animal to be added to the game field.</param>
    public void AddAnimal(IAnimal animal)
    {
        int x;
        int y;

        do
        {
            x = new Random().Next(_fieldSize.Height);
            y = new Random().Next(_fieldSize.Width);
        }
        while (_gameField[x, y] != null);

        animal.X = x;
        animal.Y = y;
        animal.Health = Constant.InitialHealth;
        _gameField[x, y] = animal;
        _animals.Add(animal);
    }
}
