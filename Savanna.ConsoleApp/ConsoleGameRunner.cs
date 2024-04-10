using CodeLibrary;
using Common.Interfaces;
using Common;
using Common.Constants;
using CodeLibrary.GameEngine;

namespace Savanna.ConsoleApp;

public class ConsoleGameRunner : IGameRunner
{
    private Game _game;
    private GameSetup _gameSetup;

    public ConsoleGameRunner() { }

    public void SetGame(Game game, GameSetup gameSetup)
    {
        _game = game;
        _gameSetup = gameSetup;
    }

    public void SetGameRunningCallback(IGameRunningCallback callback)
    {
    }

    /// <summary>
    /// This is an asynchronous method that runs the game indefinitely. It first displays the initial state of the game field. 
    /// If the count of antelopes is less than 10, it adds an antelope. If the count of lions is less than 10, it adds a lion. 
    /// Once both the antelopes and lions reach a count of 10, the game starts. Each animal in the game is moved, and the updated state of the game field is displayed. 
    /// The method then waits for a second before the next iteration.
    /// </summary>
    /// 
    public async Task Run()
    {
        while (true)
        {
            string gameState = string.Join("\n", Enumerable.Range(0, 20)
        .Select(i => new string(Enumerable.Range(0, 100)
        .Select(j => (char)_game.GameField.GetState(i, j)).ToArray())));

            _game.GameUI.Clear();
            _game.GameUI.Display(gameState);

            if (_game.AnimalOrder.Count > 0)
            {
                await _game.AddAnimalInitialSetup();
            }
            else
            {
                // Create a copy of the animals list
                var animalsCopy = new List<IAnimal>(_gameSetup.GetAnimals);
                //Start Game: 
                foreach (var animal in animalsCopy)
                {
                    _game.Logic.PlayGame(animal);
                }

                // Display the updated state of the game field
                string updatedGameState = _game.Logic.DrawField;
                _game.GameUI.Clear();
                _game.GameUI.Display(updatedGameState);
                DisplayAnimalHealth();
                await Task.Delay(100);

                var key = await _game.GameUI.GetKeyPress();
                if (key.HasValue && key.Value == ConsoleKey.S)
                {
                    _game.GameUI.Display(Constant.GameOverMessage);
                    DisplayLiveAnimalsCount();
                    _game.GameUI.Display(Constant.CloseAppMessage);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Displays the health status of each Antelope and Lion in the game.
    /// For each animal, it prints the animal's type, ID, and current health.
    /// If an animal's health is zero or less, it also prints that the animal has died.
    /// </summary>
    private void DisplayAnimalHealth()
    {
        var animalId = new Dictionary<string, int>();

        foreach (var animal in _gameSetup.GetAnimals)
        {
            if (!animalId.TryGetValue(animal.Name, out int value))
            {
                value = 1;
                animalId[animal.Name] = value;
            }
            _game.GameUI.Display(animal.Health > 0
                ? $"{animal.Name} {value}: health {animal.Health}"
                : $"{animal.Name} {value}: health {animal.Health} - died");
            animalId[animal.Name] = ++value;
        }
    }

    /// <summary>
    /// Displays the count of live Antelopes and Lions in the game.
    /// If only Antelopes or only Lions are alive, declares them as the winner.
    /// </summary>
    private void DisplayLiveAnimalsCount()
    {
        var liveAnimals = new Dictionary<string, int>();

        foreach (var animal in _gameSetup.GetAnimals)
        {
            if (animal.Health > 0)
            {
                if (!liveAnimals.TryGetValue(animal.Name, out int value))
                {
                    value = 0;
                    liveAnimals[animal.Name] = value;
                }
                liveAnimals[animal.Name] = ++value;
            }
        }

        foreach (var animal in liveAnimals.Keys)
        {
            _game.GameUI.Display($"Live {animal}s: {liveAnimals[animal]}");
        }

        if (liveAnimals.Count == 1)
        {
            _game.GameUI.Display($"{liveAnimals.Keys.First()}s won");
        }
    }
}
