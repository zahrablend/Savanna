using CodeLibrary;
using Common;
using Common.Interfaces;

namespace Savanna.Web.Services;

public class WebGameRunnerService : IGameRunner
{
    private IGameRunningCallback _callback;

    public void SetGameRunningCallback(IGameRunningCallback callback)
    {
        _callback = callback;
    }

    public async Task Run()
    {
        // Game running logic here...

        // Invoke the callback at appropriate points in the game loop
        if (_callback != null)
        {
            await _callback.Callback();
        }

        // More game running logic...
    }
}
