using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Savanna.Infrastructure;

public class GameHub : Hub
{
    private IGameEventService _gameEventService;

    public GameHub(IGameEventService gameEventService)
    {
        _gameEventService = gameEventService;
    }

    public void SendKeyPress(string key)
    {
        _gameEventService.RaiseKeyPressEvent(key);
    }
}
