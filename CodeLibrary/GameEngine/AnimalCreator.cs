using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class AnimalCreator
{
    private List<IAnimal> _animals;
    private GameLogicOrchestrator _logic;
    private Dictionary<(IAnimal, IAnimal), int> _consecutiveRounds = new();


    public AnimalCreator(GameLogicOrchestrator logic)
    {
        _logic = logic;
        _animals = new List<IAnimal>();
    }


    public void SetAnimals(List<IAnimal> animals)
    {
        _animals = animals;
    }

    /// <summary>
    /// Creates a new animal when two animals of the same type are neighbors for three consecutive rounds.
    /// </summary>
    public void CreateAnimalOnBirth()
    {
        for (int i = 0; i < _animals.Count; i++)
        {
            for (int j = i + 1; j < _animals.Count; j++)
            {
                var animal1 = _animals[i];
                var animal2 = _animals[j];

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
                        _logic.AddAnimal(newAnimal);
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

