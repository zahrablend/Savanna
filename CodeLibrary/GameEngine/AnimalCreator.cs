using Common.Factories;
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
        throw new NotImplementedException();
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

