using CodeLibrary;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Services;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using static CodeLibrary.FieldDisplayer;
using Savanna.Infrastructure;
using Savanna.Web.Models;
using CodeLibrary.GameEngine;
using Common.Entities;
using Common.Identity;
using Microsoft.AspNetCore.Identity;

namespace Savanna.Web.Controllers;

[Authorize]
public class GameController : Controller
{
    private Game _game;
    private IGameRunner _gameRunner;
    private IGameUI _gameUI;
    private GameSetup _gameSetup;
    private IGameRunningCallback _gameRunningCallback;
    private readonly GameService _gameService;
    private readonly UserManager<ApplicationUser> _userManager;

    public GameController(IGameFieldFactory gameFieldFactory, GameService gameService, UserManager<ApplicationUser> userManager, IGameRunningCallback gameRunningCallback)
    {
        _gameService = gameService;
        _userManager = userManager;
        _gameRunner = new WebGameRunnerService();
        _gameRunningCallback = gameRunningCallback;
        IGameField gameField = gameFieldFactory.Create(20, 100);
        _gameSetup = new GameSetup(gameFieldFactory, new FieldDisplayer());
        var animalFactoryLoader = new AnimalFactoryLoader();
        _gameUI = new WebGameUIService();
        _game = new Game(_gameUI, _gameRunner, gameFieldFactory, animalFactoryLoader, _gameSetup, _gameRunningCallback);
        _gameRunner.SetGameRunningCallback(_gameRunningCallback);
        _gameRunningCallback.UpdateCallback(_game.Run);
    }


    [HttpGet]
    public IActionResult GetAnimalTypes()
    {
        var animalTypes = _game.GetAnimalTypes();
        return Ok(animalTypes);
    }


    public IActionResult Play(bool isNewGame = false)
    {
        _game.SetFieldDisplayerSize(20, 100);
        List<char> animalTypes = _game.GetAnimalTypes();

        if (animalTypes.Count > 0)
        {
            string animalType = animalTypes[0].ToString();

            if (!_game.AnimalFactories.TryGetValue(animalType[0], out var factoryInfo))
            {
                return BadRequest($"No animal factory found for animal type '{animalType}'");
            }

            IAnimalFactory factory = factoryInfo.factory;
            IAnimal animal = factory.Create();

            _game.GameSetup.AddAnimal(animal);
        }

       
        ViewBag.GameField = _game.FieldDisplayer.DrawField(_game.GameField, _game.FieldDisplayer.Size.Height, _game.FieldDisplayer.Size.Width);
        var gameViewModel = new GameViewModel
        {
            GameId = _game.GameId,
            GameState = _game.GameState,
            Name = _game.Name,
            GameIteration = _game.GameIteration
        };
        return View(gameViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAnimal(string animalType)
{
    // Check if there's a factory for the given animal type
    if (!_game.AnimalFactories.TryGetValue(animalType[0], out var factoryInfo))
    {
        return BadRequest($"No animal factory found for animal type '{animalType}'");
    }

    // Create an animal of the given type
    IAnimalFactory factory = factoryInfo.factory;
    IAnimal animal = factory.Create();

    // Add the animal to the game field
    _game.GameSetup.AddAnimal(animal);

    // Get the updated game field
    var updatedGameField = _game.GameSetup.GetGameField();

    return Ok(updatedGameField);
}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartGame()
    {
        await _game.Run();
        return Ok();
    }

    [HttpGet]
    public IActionResult GetGameField()
    {
        _game.UpdateGameState();
        return Ok(_game.GameState);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveGame()
    {
        var gameEntity = new GameEntity
        {
            Id = _game.GameId,
            Name = _game.Name,
            GameState = _game.GameState,
            GameIteration = _game.GameIteration
        };
        await _gameService.SaveGameState(gameEntity);
        return Ok();
    }
}
