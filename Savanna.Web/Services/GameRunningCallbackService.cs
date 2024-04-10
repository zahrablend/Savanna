using Common.Interfaces;

namespace Savanna.Web.Services;

public class GameRunningCallbackService : IGameRunningCallback
{
    private Func<Task> _callback;

    public GameRunningCallbackService(Func<Task> callback)
    {
        _callback = callback;
    }

    public Task Callback() => _callback();

    public void UpdateCallback(Func<Task> callback)
    {
        _callback = callback;
    }
}
