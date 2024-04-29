namespace Common.Interfaces;

public interface IGameUI
{
    void Display(string message);
    Task<ConsoleKey?> GetKeyPress();
}
