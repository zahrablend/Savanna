using CodeLibrary.GameEngine;
using CodeLibrary;
using Common.Entities;
using Common.Interfaces;
using Common.ValueObjects;
using Savanna.Infrastructure;
using System.Text.Json;

namespace Savanna.Web.Services;

public class GameService
{
    private readonly IGameRepository _gameRepository;
    private readonly GameSetup _gameSetup;
    private readonly IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private readonly AnimalService _animalService;
    private readonly Dictionary<string, IAnimalFactory> _animalFactories;
    public IGameField GameField { get; private set; }

    public GameService(IGameRepository gameRepository, IGameFieldFactory gameFieldFactory, AnimalFactoryLoader animalFactoryLoader, string gameFieldState = null, AnimalService animalService = null)
    {
        // Load the animal factories dynamically
        var antelopeFactory = animalFactoryLoader.LoadAnimalFactory(
            Environment.GetEnvironmentVariable("ANTELOPEBEHAVIOUR_PATH"),
            "AntelopeBehaviour.AntelopeFactory"
        );

        var lionFactory = animalFactoryLoader.LoadAnimalFactory(
            Environment.GetEnvironmentVariable("LIONBEHAVIOUR_PATH"),
            "LionBehaviour.LionFactory"
        );

        _animalFactories = new Dictionary<string, IAnimalFactory>
        {
            { antelopeFactory.Species, antelopeFactory },
            { lionFactory.Species, lionFactory }
        };

        AnimalDictionary animalDict = new AnimalDictionary();
        animalDict.RepresentAnimal(antelopeFactory);
        animalDict.RepresentAnimal(lionFactory);

        try
        {
            _gameField = gameFieldState != null ? JsonSerializer.Deserialize<GameField>(gameFieldState) : gameFieldFactory.Create(100, 20);
        }
        catch (JsonException)
        {
            _gameField = gameFieldFactory.Create(100, 20);
        }

        _gameSetup = new GameSetup(_gameField, animalDict);
        var emptyCell = new FieldCell { State = null };
        GameField = _gameField;

        Dictionary<string, IAnimalFactory> animalFactories = new Dictionary<string, IAnimalFactory>
        {
            { antelopeFactory.Species, antelopeFactory },
            { lionFactory.Species, lionFactory }
        };

        // Only initialize the game field if it's not already initialized
        if (gameFieldState == null)
        {
            _gameField.Initialize(emptyCell);
        }

        _animalService = animalService ?? new AnimalService();

        // Initialize FieldDisplayer
        _fieldDisplayer = new FieldDisplayer(_gameSetup);
        _gameRepository = gameRepository;
    }

    public string DisplayField()
    {
        return _fieldDisplayer.DrawField(_gameField, _gameField.Height, _gameField.Width, GameSetup.DisplayType.Symbol);
    }

    public List<string> GetAnimalTypes()
    {
        return _animalFactories.Keys.ToList();
    }

    public IAnimalFactory GetAnimalFactory(string animalType)
    {
        return _animalFactories.ContainsKey(animalType) ? _animalFactories[animalType] : null;
    }


    public IAnimal AddAnimal(IAnimalFactory animalFactory)
    {
        // Delegate the task of adding an animal to the GameSetup
        return _gameSetup.AddAnimal(animalFactory);
    }


    public List<IAnimal> GetAnimals()
    {
        return _animalService.GetAnimals();
    }


    public string GetGameField()
    {
        var fieldDisplayer = new FieldDisplayer(_gameSetup);
        return fieldDisplayer.DrawField(_gameSetup.GameField, _gameSetup.GameField.Height, _gameSetup.GameField.Width, GameSetup.DisplayType.Symbol);
    }


    public GameSetup GetGameSetup()
    {
        return _gameSetup;
    }

    //Saving, getting List of Games and Loading game
    public async Task SaveGameState(GameEntity gameEntity)
    {
        await _gameRepository.SaveGame(gameEntity);
    }



    public async Task<GameEntity> LoadGameState(int id)
    {
        return await _gameRepository.LoadGame(id);
    }


    public async Task<IEnumerable<GameEntity>> GetSavedGames(Guid userId)
    {
        var games = await _gameRepository.GetGamesByUserId(userId);
        return games ?? new List<GameEntity>();
    }

}
