﻿using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalRemover
{ 
    private IAnimal?[,] _gameField;
    private List<IAnimal> _animals;

    public AnimalRemover(IAnimal?[,] gameField, List<IAnimal> animals)
    {
        _gameField = gameField;
        _animals = animals;
    }

    /// <summary>
    /// If animals health is zero or less, removes the specified animal from the game field and animals list.
    /// </summary>
    /// <param name="animal"></param>
    public void RemoveAnimalOnDeath(IAnimal animal)
    {
        if (animal.Health <= 0)
        {
            _gameField[animal.X, animal.Y] = null;
            _animals.Remove(animal);
        }
    }
}
