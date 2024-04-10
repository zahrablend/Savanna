using Common.Interfaces;
using Common.ValueObjects;

namespace CodeLibrary.GameEngine;

public class AnimalMover
{
    private IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private static readonly Random random = new();

    public AnimalMover(IGameField gameField, FieldDisplayer fieldDisplayer)
    {
        _gameField = gameField;
        _fieldDisplayer = fieldDisplayer;
    }

    /// <summary>
    /// Moves the specified animal to a new position in the game field.
    /// The new position is determined based on the animal's movement direction.
    /// If the new position is not occupied by another animal, the animal is moved to the new position.    /// </summary>
    /// <param name="animal">The animal to move.</param>
    public void MoveAnimal(IAnimal animal)
    {
        Direction direction = GetMovementDirection(animal);
        Direction newPosition = GetNewPosition(animal, direction);

        if (_gameField.GetState(newPosition.X, newPosition.Y) == null)
        {
            _gameField.SetState(animal.X, animal.Y, null);
            animal.X = newPosition.X;
            animal.Y = newPosition.Y;
            _gameField.SetState(animal.X, animal.Y, animal);
        }
    }

    private Direction GetMovementDirection(IAnimal animal)
    {
        Direction direction = new()
        {
            X = random.Next(-animal.Speed, animal.Speed + 1),
            Y = random.Next(-animal.Speed, animal.Speed + 1)
        };

        // Check for other animals within the vision range
        for (int i = Math.Max(0, animal.X - animal.VisionRange); i <= Math.Min(_fieldDisplayer.Size.Height - 1, animal.X + animal.VisionRange); i++)
        {
            for (int j = Math.Max(0, animal.Y - animal.VisionRange); j <= Math.Min(_fieldDisplayer.Size.Width - 1, animal.Y + animal.VisionRange); j++)
            {
                var otherAnimal = _gameField.GetState(i, j) as IAnimal;

                if (otherAnimal != null && otherAnimal.GetType() != animal.GetType())
                {
                    var otherDirection = animal.GetDirectionTo(otherAnimal);
                    direction.X = otherDirection.X;
                    direction.Y = otherDirection.Y;
                }
            }
        }
        return direction;
    }

    /// <summary>
    /// Calculates the new position of an animal based on its current position and the direction of movement.
    /// </summary>
    /// <param name="animal">The animal for which to calculate the new position.</param>
    /// <param name="dx">The movement direction on the x-axis.</param>
    /// <param name="dy">The movement direction on the y-axis.</param>
    /// <returns>A tuple of integers representing the new position on the x and y axes.</returns>
    private Direction GetNewPosition(IAnimal animal, Direction direction)
    {
        Direction newPosition = new()
        {
            X = (animal.X + direction.X),
            Y = (animal.Y + direction.Y)
        };
        

        // Ensure the new position is within the game field boundaries
        if (newPosition.X < 0) newPosition.X = 0;
        if (newPosition.X >= _fieldDisplayer.Size.Height) newPosition.X = _fieldDisplayer.Size.Height - 1;
        if (newPosition.Y < 0) newPosition.Y = 0;
        if (newPosition.Y >= _fieldDisplayer.Size.Width) newPosition.Y = _fieldDisplayer.Size.Width - 1;

        return newPosition;
    }
}
