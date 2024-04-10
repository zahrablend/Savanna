using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Savanna.Infrastructure;

public class GameHub : Hub, IDisposable
{
    private IGameEventService _gameEventService;
    private IGameRunner _gameRunner;
    private IHubContext<GameHub> _hubContext;

    public GameHub(IGameEventService gameEventService, IGameRunner gameRunner, IHubContext<GameHub> hubContext)
    {
        _gameEventService = gameEventService;
        _gameRunner = gameRunner;
        _hubContext = hubContext;
    }

    //public void SendKeyPress(string key)
    //{
    //    _gameEventService.RaiseKeyPressEvent(key);
    //    _gameRunner.Run();
    //    SendGameState();
    //}

    //public void RequestGameState()
    //{
    //    var gameUI = _game.GetGameUI();
    //    var gameState = gameUI.GetLastGameState();
    //    Clients.Caller.SendAsync("Display", gameState);
    //}

    //private void SendGameState()
    //{
    //    var gameUI = _game.GetGameUI();
    //    var gameState = gameUI.GetLastGameState();
    //    _hubContext.Clients.All.SendAsync("Display", gameState);
    //}
}
