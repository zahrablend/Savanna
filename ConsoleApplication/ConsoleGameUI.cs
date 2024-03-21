using CodeLibrary.Interfaces;

namespace ConsoleApplication;

public class ConsoleGameUI : IGameUI
{
    public void Clear()
    {
        Console.Clear();
    }

    public void Display(string message)
    {
        Console.WriteLine(message);
    }

    public async Task<ConsoleKey?> GetKeyPress()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.A || key == ConsoleKey.S || key == ConsoleKey.L)
            {
                return key;
            }
        }
        await Task.Delay(1000);
        return null;
    }
}
