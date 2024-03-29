using CodeLibrary.Interfaces;
using CodeLibrary;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Services;
using Savanna.Web.Models;
using Common.Interfaces;

namespace Savanna.Web.Controllers
{
    public class GameController : Controller
    {
        private Game _game;
        private IGameRunner _gameRunner;

            public GameController(IGameUI gameUI)
            {
                _game = new Game(gameUI);
                _gameRunner = new WebGameRunner();
            }

        [Route("gameplay/index")]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            await _gameRunner.Run(_game);

            var gameUI = (WebGameUI)_game.GetGameUI();

            var viewModel = new GameViewModel
            {
                GameState = gameUI.GetLastGameState(),
            };

            return View(viewModel);
        }
    }
}
