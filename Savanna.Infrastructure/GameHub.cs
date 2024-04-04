using CodeLibrary;
using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Savanna.Infrastructure;

public class GameHub : Hub, IDisposable
{
    private IGameEventService _gameEventService;
    private Game _game;
    private Timer _timer;
    private IHubContext<GameHub> _hubContext;

    public GameHub(IGameEventService gameEventService, Game game, IHubContext<GameHub> hubContext)
    {
        _gameEventService = gameEventService;
        _game = game;
        _hubContext = hubContext;
        _timer = new Timer(SendGameState, null, 0, 1000);
    }

    public void SendKeyPress(string key)
    {
        _gameEventService.RaiseKeyPressEvent(key);
    }

    public void RequestGameState()
    {
        var gameUI = _game.GetGameUI();
        var gameState = gameUI.GetLastGameState();
        Clients.Caller.SendAsync("Display", gameState);
    }

    private void SendGameState(object state)
    {
        var gameUI = _game.GetGameUI();
        var gameState = gameUI.GetLastGameState();
        _hubContext.Clients.All.SendAsync("Display", gameState);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
