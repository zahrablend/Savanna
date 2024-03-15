using CodeLibrary.Animals;
using CodeLibrary.GameEngine;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class Game
{
    private char[,] _gameField;
    private int _antelopeCount;
    private int _lionCount;
    private Random _random;
    private readonly GameLogicOrchestrator _logic;
    private readonly FieldDisplayer _fieldDisplayer;
    private List<IAnimal> _animals;

    public Game()
    {
        _gameField = new char[20, 100];
        for (int i = 0; i < _gameField.GetLength(0); i++)
        {
            for (int j = 0; j < _gameField.GetLength(1); j++)
            {
                _gameField[i, j] = '.';
            }
        }
        _antelopeCount = 0;
        _lionCount = 0;
        _random = new Random();
        _fieldDisplayer = new FieldDisplayer();
        _logic = new GameLogicOrchestrator(new FieldDisplayer.FieldSize(20,100), _fieldDisplayer);
        _animals = new List<IAnimal>();
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
            // Convert each row of the game field to a string and join them with newlines
            string gameState = string.Join("\n", Enumerable.Range(0, _gameField.GetLength(0))
                .Select(i => new string(Enumerable.Range(0, _gameField.GetLength(1))
                .Select(j => _gameField[i, j]).ToArray())));

            Console.Clear(); // Clear the console
            Console.WriteLine(gameState);

            if (_antelopeCount < 10)
            {
                await AddAnimalInitialSetup('A');
            }
            else if (_lionCount < 10)
            {
                await AddAnimalInitialSetup('L');
            }
            else
            {
                // Create a copy of the animals list
                var animalsCopy = new List<IAnimal>(_logic.GetAnimals);
                //Start Game: 
                foreach (var animal in animalsCopy)
                {
                    _logic.PlayGame(animal);
                }

                // Display the updated state of the game field
                string updatedGameState = _logic.DrawField;
                Console.Clear();
                Console.WriteLine(updatedGameState);
                DisplayAnimalHealth();
                await Task.Delay(1000);
                if (Console.KeyAvailable)
                {
                    var key = await GetKeyPress();
                    if (key == ConsoleKey.S)
                    {
                        Console.WriteLine("Game Over");
                        DisplayLiveAnimalsCount();
                        Console.WriteLine("Press Esc key to close application");
                        break;
                    }
                }
            }
        }
    }

    private async Task AddAnimalInitialSetup(char animal)
    {
        if ((animal == 'A' && _antelopeCount >= 10) || (animal == 'L' && _lionCount >= 10))
        {
            return;
        }

        Console.WriteLine($"Add {animal} to game field. Press {animal} to continue or S to skip.");
        ConsoleKey key = await GetKeyPress();
        if (key.ToString().ToUpper() == animal.ToString())
        {
            int indexX, indexY;
            do
            {
                indexX = _random.Next(_gameField.GetLength(0));
                indexY = _random.Next(_gameField.GetLength(1));
            } while (_gameField[indexX, indexY] != '.');
            _gameField[indexX, indexY] = animal;
            if (animal == 'A')
            {
                _antelopeCount++;
            }
            else if (animal == 'L')
            {
                _lionCount++;
            }

            IAnimal gameAnimal = animal == 'A' ? new Antelope() : new Lion();
            gameAnimal.X = indexX;
            gameAnimal.Y = indexY;
            _logic.AddAnimal(gameAnimal);
        }
        else if (key == ConsoleKey.S && ((animal == 'A' && _antelopeCount >= 2) || (animal == 'L' && _lionCount >= 2)))
        {
            if (animal == 'A')
            {
                _antelopeCount = 10; 
            }
            else if (animal == 'L')
            {
                _lionCount = 10;
            }
        }
    }

    private static async Task<ConsoleKey> GetKeyPress()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.A || key == ConsoleKey.S || key == ConsoleKey.L)
                {
                    return key;
                }
            }
            await Task.Delay(100);
        }
    }

    private void DisplayAnimalHealth()
    {
        int antelopeId = 1;
        int lionId = 1;

        foreach (var animal in _logic.GetAnimals)
        {
            if (animal is Antelope && antelopeId <= _antelopeCount)
            {
                Console.WriteLine(animal.Health > 0 ? $"Antelope {antelopeId}: health {animal.Health}" : $"Antelope {antelopeId}: health {animal.Health} - died");
                antelopeId++;
            }
            else if (animal is Lion && lionId <= _lionCount)
            {
                Console.WriteLine(animal.Health > 0 ? $"Lion {lionId}: health {animal.Health}" : $"Lion {lionId}: health {animal.Health} -  died");
                lionId++;
            }
        }
    }
    private void DisplayLiveAnimalsCount()
    {
        var animals = _logic.GetAnimals;
        var liveAntelopes = animals.Count(a => a is Antelope && a.Health > 0);
        var liveLions = animals.Count(a => a is Lion && a.Health > 0);

        Console.WriteLine($"Live Antelopes: {liveAntelopes}");
        Console.WriteLine($"Live Lions: {liveLions}");

        if (liveAntelopes > 0 && liveLions == 0)
        {
            Console.WriteLine("Antelopes won");
        }
        else if (liveLions > 0 && liveAntelopes == 0)
        {
            Console.WriteLine("Lions won");
        }
    }

}


