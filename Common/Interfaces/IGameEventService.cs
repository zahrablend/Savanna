namespace Common.Interfaces;

public interface IGameEventService
{
    event Action<string> KeyPressed;
    void RaiseKeyPressEvent(string key);
}
