using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalCreator
{
    private GameSetup _gameSetup;
    private Dictionary<(IAnimal, IAnimal), int> _consecutiveRounds = new();


    public AnimalCreator(GameSetup gameSetup)
    {
        _gameSetup  = gameSetup;
    }

    /// <summary>
    /// Creates a new animal when two animals of the same type are neighbors for three consecutive rounds.
    /// </summary>
    public void CreateAnimalOnBirth()
    {
        var animals = _gameSetup.GetAnimals;

        for (int i = 0; i < animals.Count; i++)
        {
            for (int j = i + 1; j < animals.Count; j++)
            {
                var animal1 = animals[i];
                var animal2 = animals[j];

                if (animal1.GetType() == animal2.GetType() && AreNeighbours(animal1, animal2))
                {
                    var pair = (animal1, animal2);

                    if (_consecutiveRounds.ContainsKey(pair))
                    {
                        _consecutiveRounds[pair]++;
                    }
                    else
                    {
                        _consecutiveRounds[pair] = 1;
                    }

                    if (_consecutiveRounds[pair] >= 3)
                    {
                        var newAnimal = (IAnimal)Activator.CreateInstance(animal1.GetType());
                        _gameSetup.AddAnimal(newAnimal);
                        _consecutiveRounds[pair] = 0;
                    }
                }
                else
                {
                    _consecutiveRounds.Remove((animal1, animal2));
                    _consecutiveRounds.Remove((animal2, animal1));
                }
            }
        }
    }

    /// <summary>
    /// Determines whether two animals are neighbors.
    /// </summary>
    /// <param name="animal1">The first animal.</param>
    /// <param name="animal2">The second animal.</param>
    /// <returns>true if the animals are neighbors, false otherwise.</returns>
    private static bool AreNeighbours(IAnimal animal1, IAnimal animal2)
    {
        return Math.Abs(animal1.X - animal2.X) <= 1
            && Math.Abs(animal1.Y - animal2.Y) <= 1;
    }
}

