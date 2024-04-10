namespace Common.Interfaces;

public interface IGameUI : IGameStateSender
{
    void Display(string message);
    void Clear();
    Task<ConsoleKey?> GetKeyPress();
    string GetLastGameState();
}
