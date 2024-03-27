using CodeLibrary;

namespace Savanna.ConsoleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var game = new Game(new ConsoleGameUI());
        await game.Run();
        Console.ReadKey();
    }
}
