﻿using CodeLibrary.Animals;
using CodeLibrary.Constants;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class GameEngine
{
    private IAnimal?[,] _gameField; //not readonly!
    private List<IAnimal> _animals; //not readonly!
    private readonly FieldDisplayer.FieldSize _fieldSize;
    private readonly FieldDisplayer _fieldDisplayer;
    private static readonly Random random = new();

    public GameEngine(FieldDisplayer.FieldSize fieldSize, FieldDisplayer fieldDisplayer)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = [];
        _fieldDisplayer = fieldDisplayer;
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
        while(_gameField[x, y] != null);

        animal.X = x;
        animal.Y = y;
        _gameField[x, y] = animal;
        _animals.Add(animal);
        animal.Health = Constant.InitialHealth;
    }

    /// <summary>
    /// Moves an animal on the game field based on its speed and vision range.
    /// </summary>
    /// <param name="animal">The animal to be moved.</param>
    public void MoveAnimal(IAnimal animal)
    {
        var (dx, dy) = GetMovementDirection(animal);
        var (newX, newY) = GetNewPosition(animal, dx, dy);

        if (_gameField[newX, newY] == null)
        {
            _gameField[animal.X, animal.Y] = null;
            animal.X = newX;
            animal.Y = newY;
            _gameField[animal.X, animal.Y] = animal;
        }

        // Decrease health by HealthDecreasePerMove with each move
        animal.Health -= Constant.HealthDecreasePerMove;

        // Check the neighboring cells for other animals
        for (int i = Math.Max(0, animal.X - 1); i <= Math.Min(_fieldSize.Height - 1, animal.X + 1); i++)
        {
            for (int j = Math.Max(0, animal.Y - 1); j <= Math.Min(_fieldSize.Width - 1, animal.Y + 1); j++)
            {
                var otherAnimal = _gameField[i, j];
                if (otherAnimal != null)
                {
                    if (animal is Lion && otherAnimal is Antelope)
                    {
                        // Increase the lion's health by 1
                        animal.Health += 1;
                        // Decrease the antelope's health to 0
                        otherAnimal.Health = 0;
                    }
                    else if (animal.GetType() == otherAnimal.GetType())
                    {
                        // Increase the count if the neighbor is of the same type
                        animal.SameTypeNeighborCount++;
                    }
                }
            }
        }

        // If the animal's health reaches 0, remove it from the game field
        if (animal.Health <= 0)
        {
            _gameField[animal.X, animal.Y] = null;
            _animals.Remove(animal);
        }
    }

    /// <summary>
    /// Calculates the direction in which an animal should move based on its speed and vision range.
    /// </summary>
    /// <param name="animal">The animal for which to calculate the movement direction.</param>
    /// <returns>A tuple of integers representing the movement direction on the x and y axes.</returns>
    private (int dx, int dy) GetMovementDirection(IAnimal animal)
    {
        int dx = random.Next(-animal.Speed, animal.Speed + 1);
        int dy = random.Next(-animal.Speed, animal.Speed + 1);

        // Check for other animals within the vision range
        for (int i = Math.Max(0, animal.X - animal.VisionRange); i <= Math.Min(_fieldSize.Height - 1, animal.X + animal.VisionRange); i++)
        {
            for (int j = Math.Max(0, animal.Y - animal.VisionRange); j <= Math.Min(_fieldSize.Width - 1, animal.Y + animal.VisionRange); j++)
            {
                var otherAnimal = _gameField[i, j];
                if (otherAnimal != null && otherAnimal.GetType() != animal.GetType())
                {
                    // If the animal is an Antelope and it sees a Lion, it moves away
                    if (animal is Antelope && otherAnimal is Lion)
                    {
                        dx = animal.X > otherAnimal.X ? animal.Speed : -animal.Speed;
                        dy = animal.Y > otherAnimal.Y ? animal.Speed : -animal.Speed;
                    }

                    // If the animal is a Lion and it sees an Antelope, it moves towards it
                    else if (animal is Lion && otherAnimal is Antelope)
                    {
                        dx = animal.X < otherAnimal.X ? animal.Speed : -animal.Speed;
                        dy = animal.Y < otherAnimal.Y ? animal.Speed : -animal.Speed;
                    }
                }
            }
        }

        return (dx, dy);
    }

    /// <summary>
    /// Calculates the new position of an animal based on its current position and the direction of movement.
    /// </summary>
    /// <param name="animal">The animal for which to calculate the new position.</param>
    /// <param name="dx">The movement direction on the x-axis.</param>
    /// <param name="dy">The movement direction on the y-axis.</param>
    /// <returns>A tuple of integers representing the new position on the x and y axes.</returns>
    private (int newX, int newY) GetNewPosition(IAnimal animal, int dx, int dy)
    {
        int newX = (animal.X + dx) % _fieldSize.Height;
        int newY = (animal.Y + dy) % _fieldSize.Width;

        // Ensure the new position is within the game field boundaries
        if (newX < 0) newX = 0;
        if (newX >= _fieldSize.Height) newX = _fieldSize.Height - 1;
        if (newY < 0) newY = 0;
        if (newY >= _fieldSize.Width) newY = _fieldSize.Width - 1;

        return (newX, newY);
    }

    public string DrawField()
    {
        return _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    }

    public IAnimal[,] GetGameField()
    {
        return _gameField;
    }

    public List<IAnimal> GetAnimals()
    {
        return _animals;
    }
}
