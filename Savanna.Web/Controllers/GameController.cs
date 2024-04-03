using CodeLibrary.Interfaces;
using CodeLibrary;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Services;
using Savanna.Web.Models;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Common.Entities;
using System.Security.Claims;

namespace Savanna.Web.Controllers;

[Authorize]
public class GameController : Controller
{
    private Game _game;
    private IGameRunner _gameRunner;
    private readonly GameService _gameService;

    public GameController(IGameUI gameUI, GameService gameService)
        {
            _game = new Game(gameUI);
            _gameRunner = new WebGameRunner();
            _gameService = gameService;
    }

    [Route("game/play")]
    [HttpGet]
    public async Task<IActionResult> Play(bool isNewGame, int id = 0)
    {
        GameEntity gameEntity;
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (isNewGame)
        {
            await _gameRunner.Run(_game);
            var gameUI = (WebGameUI)_game.GetGameUI();
            var gameName = $"Game_{userId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            gameEntity = new GameEntity(userId)
            {
                Name = gameName,
                GameState = gameUI.GetLastGameState(),
                GameIteration = _game.GameIteration
        };
        }
        else
        {
            gameEntity = await _gameService.LoadGameState(id);
            _game.SetState(gameEntity.GameState);
            _game.SetIteration(gameEntity.GameIteration);
        }

        var viewModel = new GameViewModel
        {
            GameId = gameEntity.Id,
            GameState = gameEntity.GameState,
            Name = gameEntity.Name,
            GameIteration = gameEntity.GameIteration
        };

        if (isNewGame)
        {
            return View(viewModel);
        }
        else
        {
            var savedGames = await _gameService.GetSavedGames(userId);
            if (!savedGames.Any())
            {
                TempData["Message"] = "There are no saved games yet.";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                return PartialView("_LoadGame", savedGames);

            }
        }
    }


    [HttpPost]
    [Route("game/save")]
    public async Task<IActionResult> SaveGame(GameViewModel model)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
        {
            ModelState.AddModelError("", "Invalid user ID");
            return View(model);
        }

        var gameName = $"Game_{userId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        
        var gameEntity = new GameEntity(userId)
        {
            Id = model.GameId,
            Name = gameName,
            GameState = model.GameState,
            GameIteration = model.GameIteration
        };
        await _gameService.SaveGameState(gameEntity);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    //text
    [HttpGet]
    [Route("game/state")]
    public IActionResult GetGameState()
    {
        var gameUI = (WebGameUI)_game.GetGameUI();
        var gameState = gameUI.GetLastGameState();
        return Ok(gameState);
    }
}
