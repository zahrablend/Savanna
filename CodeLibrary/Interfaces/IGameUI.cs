namespace CodeLibrary.Interfaces;

public interface IGameUI
{
    void Display(string message);
    void Clear();
    Task<ConsoleKey?> GetKeyPress();
}
