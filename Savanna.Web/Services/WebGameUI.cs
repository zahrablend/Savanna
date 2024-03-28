using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Savanna.Infrastructure;
using System.Collections.Concurrent;

namespace Savanna.Web.Services;

public class WebGameUI : IGameUI, IGameEventService
{
    private IHubContext<GameHub> _hubContext;
    private ConcurrentQueue<ConsoleKey?> _keyPresses = new ConcurrentQueue<ConsoleKey?>();
    private string _lastGameState;

    public event Action<string> KeyPressed = delegate { };

    public WebGameUI(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }


    public void Clear()
    {
        _lastGameState = null;
        _hubContext.Clients.All.SendAsync("Clear");
    }


    public void Display(string message)
    {
        _lastGameState = message; // Store the game state
        _hubContext.Clients.All.SendAsync("Display", message);
    }


    public string GetLastGameState()
    {
        return _lastGameState;
    }


    public async Task<ConsoleKey?> GetKeyPress()
    {
        if (_keyPresses.TryDequeue(out var keyPress))
        {
            return keyPress;
        }
        else
        {
            await Task.Delay(1000);
            return null;
        }
    }

    public void RaiseKeyPressEvent(string key)
    {
        if (Enum.TryParse(key, out ConsoleKey consoleKey))
        {
            _keyPresses.Enqueue(consoleKey);
        }

        KeyPressed.Invoke(key);
    }
}
