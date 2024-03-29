using CodeLibrary;

namespace Savanna.ConsoleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var game = new Game(new ConsoleGameUI());
        var gameRunner = new ConsoleGameRunner();
        await gameRunner.Run(game);
        Console.ReadKey();
    }
}
