using CodeLibrary;
using CodeLibrary.Interfaces;
using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Savanna.Infrastructure;

public class GameHub : Hub
{
    private IGameEventService _gameEventService;
    private Game _game;

    public GameHub(IGameEventService gameEventService, Game game)
    {
        _gameEventService = gameEventService;
        _game = game;
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
}
