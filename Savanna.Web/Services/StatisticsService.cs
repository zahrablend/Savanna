using Common.Interfaces;
using Savanna.Web.Models;

namespace Savanna.Web.Services;

public class StatisticsService
{
    private readonly IGameRepository _gameRepository;
    private readonly IAnimalRepository _animalRepository;

    public StatisticsService(IGameRepository gameRepository, IAnimalRepository animalRepository)
    {
        _gameRepository = gameRepository;
        _animalRepository = animalRepository;
    }

    public GameStatsViewModel GetGameStats(int gameId)
    {
        // Fetch game statistics from the game repository
        var game = _gameRepository.LoadGame(gameId).Result;

        // Map them to the GameStatsViewModel
        var gameStats = new GameStatsViewModel
        {
            GameId = game.Id,
            GameIteration = game.GameIteration,
            Animals = game.Animals.Select(a => GetAnimalStats(a.AnimalId)).ToList()
        };

        return gameStats;
    }


    public AnimalStatsViewModel GetAnimalStats(int animalId)
    {
        // Fetch animal statistics from the animal repository
        var animal = _animalRepository.GetAnimal(animalId).Result;

        // Map them to the AnimalStatsViewModel
        var animalStats = new AnimalStatsViewModel
        {
            AnimalId = animal.AnimalId,
            Species = animal.Species,
            Age = animal.Age,
            Health = animal.Health,
            Offspring = animal.Offspring
        };

        return animalStats;
    }
}



