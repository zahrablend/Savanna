using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Savanna.Infrastructure;
using System.Collections.Concurrent;

namespace Savanna.Web.Services;

public class WebGameUI : IGameUI, IGameEventService, IGameStateSender
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
       
    }


    public void Display(string message)
    {
        _lastGameState = message;
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
        if (Enum.TryParse(key, out ConsoleKey consoleKey) 
            && (consoleKey == ConsoleKey.A 
            || consoleKey == ConsoleKey.Z 
            || consoleKey == ConsoleKey.L 
            || consoleKey == ConsoleKey.S))
        {
            _keyPresses.Enqueue(consoleKey);
        }
        KeyPressed.Invoke(key);
    }

    public void SendGameState(string gameState)
    {
        _hubContext.Clients.All.SendAsync("Display", gameState);
    }
}
