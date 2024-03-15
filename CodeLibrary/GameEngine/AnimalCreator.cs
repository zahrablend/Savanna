﻿using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public  class AnimalCreator
{
    private List<IAnimal> _animals;
    private GameLogicOrchestrator _logic;

    public AnimalCreator( GameLogicOrchestrator logic)
    {
        _logic = logic;
    }

    private Dictionary<IAnimal, int> _consecutiveRounds = new();
    public void CreateAnimalOnBirth(IAnimal animal)
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
            _logic.AddAnimal(newAnimals[i]);
        }
    }

    private static bool AreNeighbours(IAnimal animal1, IAnimal animal2)
    {
        return Math.Abs(animal1.X - animal2.X) <= 1
            && Math.Abs(animal1.Y - animal2.Y) <= 1;
    }
}
