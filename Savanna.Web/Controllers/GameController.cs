using CodeLibrary;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Services;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Savanna.Web.Models;
using CodeLibrary.GameEngine;
using Common.Entities;
using Common.Identity;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace Savanna.Web.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly GameService _gameService;
    private readonly GameSetup _gameSetup;
    private readonly AnimalService _animalService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly StatisticsService _statisticsService;

    //public GameController(GameService gameService, AnimalService animalService, GameSetup gameSetup, UserManager<ApplicationUser> userManager, StatisticsService statisticsService)
    //{
    //    _gameService = gameService;
    //    _animalService = animalService;
    //    _gameSetup = gameSetup;
    //    _userManager = userManager;
    //    _statisticsService = statisticsService;
    //}


    //[HttpGet]
    //public IActionResult GetAnimalTypes()
    //{
    //    var animalTypes = _gameService.GetAnimalTypes();
    //    return Ok(animalTypes);
    //}

    //[HttpPost]
    //public IActionResult AddAnimal(string animalType)
    //{
    //    IAnimalFactory animalFactory = _gameSetup.GetAnimalFactoryBySpecies(animalType);
    //    if (animalFactory == null)
    //    {
    //        return BadRequest($"No factory found for species {animalType}");
    //    }
    //    _gameService.AddAnimal(animalFactory);
    //    var updatedGameField = _gameService.GetGameField();
    //    return Json(updatedGameField);
    //}

    ////[HttpPost]
    ////public IActionResult AddAnimal(string animalType)
    ////{
    ////    if (!_game.AnimalFactories.TryGetValue(animalType[0], out var factoryInfo))
    ////    {
    ////        return BadRequest($"No animal factory found for animal type '{animalType}'");
    ////    }
    ////    IAnimalFactory factory = factoryInfo.factory;
    ////    IAnimal animal = factory.Create();

    ////    _game.GameSetup.AddAnimal(animal);

    ////    var updatedGameField = _game.GameSetup.GetGameField();

    ////    return Ok(updatedGameField);
    ////}

    //[HttpGet]
    //public IActionResult GetAnimalStats(int animalId)
    //{
    //    // Fetch the animal's statistics from the StatisticsService
    //    AnimalStatsViewModel animalStats = _statisticsService.GetAnimalStats(animalId);

    //    // Return the _AnimalStats partial view with the animal's statistics
    //    return PartialView("_AnimalStats", animalStats);
    //}

    //[HttpPost]
    //public IActionResult StartGame()
    //{
    //    var gameLogicOrchestrator = new GameLogicOrchestrator(_gameService.GameField, new FieldDisplayer(_gameSetup), _gameService.GetGameSetup());
    //    foreach (var animal in _gameService.GetAnimals())
    //    {
    //        gameLogicOrchestrator.PlayGame(animal);
    //    }
    //    var updatedGameField = _gameService.GetGameField();
    //    return Json(updatedGameField);
    //}


    //public IActionResult Play(bool isNewGame = false)
    //{
    //    List<string> animalTypes = _gameService.GetAnimalTypes();
    //    IAnimalFactory animalFactory = _gameSetup.GetAnimalFactoryBySpecies(species);
    //    if (animalTypes.Count > 0)
    //    {
    //        string animalType = animalTypes[0].ToString();
    //        _gameSetup.AddAnimal(animalFactory);
    //    }

    //    ViewBag.GameField = _fieldDisplayer.DrawField(_game.GameField, _game.FieldDisplayer.Size.Height, _game.FieldDisplayer.Size.Width);
    //    var gameViewModel = new GameViewModel
    //    {
    //        GameId = _game.GameId,
    //        GameState = _game.GameState,
    //        Name = _game.Name,
    //        GameIteration = _game.GameIteration
    //    };
    //    return View(gameViewModel);
    //}



    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> SaveGame()
    //{
    //    var userId = GetUserId();
    //    var gameEntity = new GameEntity
    //    {
    //        Id = _game.GameId,
    //        Name = $"{userId}_{_game.GameId}_{DateTime.Now:yyyyMMddHHmmss}",
    //        GameState = _game.GameState,
    //        GameIteration = _game.GameIteration
    //    };
    //    await _gameService.SaveGameState(gameEntity);
    //    return Ok();
    //}

    //[HttpGet]
    //public async Task<IActionResult> LoadGame()
    //{
    //    var userId = GetUserId();
    //    IEnumerable<GameEntity> savedGamesEntities = await _gameService.GetSavedGames(userId);
    //    List<GameViewModel> savedGames = savedGamesEntities.Select(gameEntity => new GameViewModel
    //    {
    //        GameId = gameEntity.Id,
    //        Name = gameEntity.Name,
    //        GameState = gameEntity.GameState,
    //        GameIteration = gameEntity.GameIteration
    //    }).ToList();

    //    return PartialView("_LoadGame", savedGames);
    //}


    //private Guid GetUserId()
    //{
    //    var userIdString = _userManager.GetUserId(User);
    //    if (Guid.TryParse(userIdString, out var userId))
    //    {
    //        return userId;
    //    }
    //    else
    //    {
    //        return Guid.Empty;
    //    }
    //}

}
