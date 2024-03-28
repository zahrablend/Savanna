using CodeLibrary;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Models;
using Savanna.Web.Services;

namespace Savanna.Web.Controllers
{
    public class GameplayController : Controller
    {
        private Game _game;
        private IGameUI _gameUI;
        private IGameEventService _gameEventService;

        public GameplayController(Game game, IGameUI gameUI, IGameEventService gameEventService)
        {
            _game = game;
            _gameUI = gameUI;
            _gameEventService = gameEventService;
        }

        [Route("gameplay/index")]
        [Route("/")]
        public IActionResult Index()
        {
            // Get the current game state
            string gameState = _gameUI.GetLastGameState();

            // Create a new GameViewModel
            var model = new GameplayViewModel
            {
                GameState = gameState
                // Set any other data you need to display in the view
            };

            // Pass the game state to the view
            return View("Index", model);
        }
    }
}
