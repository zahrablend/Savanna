namespace Common.Interfaces;

public interface IGameRunningCallback
{
    Task Callback();
    void UpdateCallback(Func<Task> callback);
}

