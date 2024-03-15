using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalMover
{
    private IAnimal?[,] _gameField;
    private readonly FieldDisplayer.FieldSize _fieldSize;
    private static readonly Random random = new();

    public AnimalMover(IAnimal?[,] gameField, FieldDisplayer.FieldSize fieldSize)
    {
        _gameField = gameField;
        _fieldSize = fieldSize;
    }
    public void MoveAnimal(IAnimal animal)
    {
        var (directionX, directionY) = GetMovementDirection(animal);
        var (newX, newY) = GetNewPosition(animal, directionX, directionY);

        if (_gameField[newX, newY] == null)
        {
            _gameField[animal.X, animal.Y] = null;
            animal.X = newX;
            animal.Y = newY;
            _gameField[animal.X, animal.Y] = animal;
        }
    }

    private (int directionX, int directionY) GetMovementDirection(IAnimal animal)
    {
        int directionX = random.Next(-animal.Speed, animal.Speed + 1);
        int directionY = random.Next(-animal.Speed, animal.Speed + 1);

        // Check for other animals within the vision range
        for (int i = Math.Max(0, animal.X - animal.VisionRange); i <= Math.Min(_fieldSize.Height - 1, animal.X + animal.VisionRange); i++)
        {
            for (int j = Math.Max(0, animal.Y - animal.VisionRange); j <= Math.Min(_fieldSize.Width - 1, animal.Y + animal.VisionRange); j++)
            {
                var otherAnimal = _gameField[i, j];
                if (otherAnimal != null && otherAnimal.GetType() != animal.GetType())
                {
                    if (animal is Antelope && otherAnimal is Lion)
                    {
                        directionX = animal.X > otherAnimal.X ? animal.Speed : -animal.Speed;
                        directionY = animal.Y > otherAnimal.Y ? animal.Speed : -animal.Speed;
                    }
                    else if (animal is Lion && otherAnimal is Antelope)
                    {
                        directionX = animal.X < otherAnimal.X ? animal.Speed : -animal.Speed;
                        directionY = animal.Y < otherAnimal.Y ? animal.Speed : -animal.Speed;
                    }
                }
            }
        }
        return (directionX, directionY);
    }

    /// <summary>
    /// Calculates the new position of an animal based on its current position and the direction of movement.
    /// </summary>
    /// <param name="animal">The animal for which to calculate the new position.</param>
    /// <param name="dx">The movement direction on the x-axis.</param>
    /// <param name="dy">The movement direction on the y-axis.</param>
    /// <returns>A tuple of integers representing the new position on the x and y axes.</returns>
    private (int newX, int newY) GetNewPosition(IAnimal animal, int directionX, int directionY)
    {
        int newX = (animal.X + directionX);
        int newY = (animal.Y + directionY);

        // Ensure the new position is within the game field boundaries
        if (newX < 0) newX = 0;
        if (newX >= _fieldSize.Height) newX = _fieldSize.Height - 1;
        if (newY < 0) newY = 0;
        if (newY >= _fieldSize.Width) newY = _fieldSize.Width - 1;

        return (newX, newY);
    }
}
