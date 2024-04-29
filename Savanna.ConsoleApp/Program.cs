using CodeLibrary;
using CodeLibrary.GameEngine;
using Common.Interfaces;
using Common.ValueObjects;
using Savanna.Infrastructure;

namespace Savanna.ConsoleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IGameField gameField = new GameField(100, 20);
        AnimalDictionary animalDict = new AnimalDictionary();
        GameSetup gameSetup = new GameSetup(gameField, animalDict);

        FieldDisplayer fieldDisplayer = new FieldDisplayer(gameSetup)
        {
            Size = new FieldDisplayer.FieldSize(20, 100)
        };

        AnimalFactoryLoader animalFactoryLoader = new AnimalFactoryLoader();
        IGameUI gameUI = new ConsoleGameUI();
        IGameUI consoleGameUI = new ConsoleGameUI();
        ConsoleApp consoleApp = new ConsoleApp(fieldDisplayer, animalFactoryLoader, gameUI, gameField, consoleGameUI);
        await consoleApp.RunGame();
    }
}