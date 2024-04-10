using CodeLibrary;
using CodeLibrary.GameEngine;
using Savanna.Infrastructure;

namespace Savanna.ConsoleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var gameRunner = new ConsoleGameRunner();
        var gameFieldFactory = new CharGameFieldFactory();
        var animalFactoryLoader = new AnimalFactoryLoader();
        var gameSetup = new GameSetup(gameFieldFactory, new FieldDisplayer());
        var game = new Game(new ConsoleGameUI(), gameRunner, gameFieldFactory, animalFactoryLoader, gameSetup, null);
        gameRunner.SetGame(game, gameSetup);
        await game.Run();
        Console.ReadKey();
    }
}