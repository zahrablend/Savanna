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

    public GameEngine()
    {    
    }

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
        animal.Health = Constant.InitialHealth;
        _gameField[x, y] = animal;
        _animals.Add(animal);
        //animal.Health = Constant.InitialHealth;
    }

    public virtual void MoveAnimal(IAnimal animal)
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

        animal.Health -= Constant.HealthDecreasePerMove;

        InteractWith(animal);
        CreateAnimalOnBirth(animal);
        RemoveAnimalOnDeath(animal);

    }

    private void InteractWith(IAnimal animal)
    {
        for (int i = Math.Max(0, animal.X - 1); i <= Math.Min(_fieldSize.Height - 1, animal.X + 1); i++)
        {
            for (int j = Math.Max(0, animal.Y - 1); j <= Math.Min(_fieldSize.Width - 1, animal.Y + 1); j++)
            {
                var otherAnimal = _gameField[i, j];
                if (otherAnimal != null)
                {
                    if (animal is Lion && otherAnimal is Antelope)
                    {
                        animal.Health += 1;
                        otherAnimal.Health = 0;
                    }
                }
            }
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
                if (otherAnimal != null && otherAnimal.GetType() !=  animal.GetType())
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

    private void RemoveAnimalOnDeath(IAnimal animal)
    {
        if (animal.Health < 0)
        {
            _gameField[animal.X, animal.Y] = null;
            _animals.Remove(animal);
        }
    }

    private Dictionary<IAnimal, int> _consecutiveRounds = new();
    private void CreateAnimalOnBirth(IAnimal animal)
    {
        var newAnimals = new List<IAnimal>();

        for (int index = 0; index < _animals.Count; index++)
        {
            var otherAnimal = _animals[index];

            if (otherAnimal.GetType() == animal.GetType()
                && AreNeighbours(animal, otherAnimal))
            {
                if (_consecutiveRounds.ContainsKey(animal))
                {
                    _consecutiveRounds[animal]++;
                }
                else
                {
                    _consecutiveRounds[animal] = 1;
                }

                if (_consecutiveRounds[animal] >= 3)
                {
                    var newAnimal = (IAnimal)Activator.CreateInstance(animal.GetType());
                    newAnimals.Add(newAnimal);
                    _consecutiveRounds[animal] = 0;
                }
            }
            else
            {
                if (_consecutiveRounds.ContainsKey(animal))
                {
                    _consecutiveRounds[animal] = 0;
                }
            }
        }

        for (int i = 0; i < newAnimals.Count; i++)
        {
            AddAnimal(newAnimals[i]);
        }
    }

    private bool AreNeighbours(IAnimal animal1, IAnimal animal2)
    {
        return Math.Abs(animal1.X - animal2.X) <= 1 
            && Math.Abs(animal1.Y - animal2.Y) <= 1;
    }

    public string DrawField => _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    public List<IAnimal> GetAnimals => _animals;
    public IAnimal[,] GetGameField => _gameField;
}
