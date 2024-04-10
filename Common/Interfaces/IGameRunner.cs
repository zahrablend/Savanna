namespace Common.Interfaces;

public interface IGameRunner
{
    void SetGameRunningCallback(IGameRunningCallback callback);
    Task Run();
}
