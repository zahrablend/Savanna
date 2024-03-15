using CodeLibrary;
using CodeLibrary.Animals;
using CodeLibrary.GameEngine;
using CodeLibrary.Interfaces;

namespace ConsoleApplication;

public static class Program
{
    private static IAnimal?[,] _gameField;
    private static int _antelopeCount;
    private static int _lionCount;
    private static Random _random;
    private static GameLogic _gameLogic;
    private static AnimalAdder _animalAdder;
    private static List<IAnimal> _animals;


    public static async Task Main(string[] args)
    {
        _gameField = new IAnimal?[20, 100];
        for (int i = 0; i < _gameField.GetLength(0); i++)
        {
            for (int j = 0; j < _gameField.GetLength(1); j++)
            {
                _gameField[i, j] = null;
            }
        }
        
        var fieldSize = new FieldDisplayer.FieldSize(20, 100);
        var fieldDisplayer = new FieldDisplayer();
        var animalMover = new AnimalMover(_gameField, fieldSize);
        var healthMetricCounter = new HealthMetricCounter(_gameField, fieldSize);
        var animalRemover = new AnimalRemover(_gameField, _animals);
        _animals = new List<IAnimal>();
        _antelopeCount = 0;
        _lionCount = 0;
        _random = new Random();
        _animalAdder = new AnimalAdder(_gameField, fieldSize);
        _gameLogic = new GameLogic(fieldSize, fieldDisplayer, animalMover, healthMetricCounter, animalRemover);


        await Run();
        Console.ReadKey();
    }
    public static async Task Run()
    {
        while (true)
        {
            string gameState = string.Join("\n", Enumerable.Range(0, _gameField.GetLength(0))
    .Select(i => new string(Enumerable.Range(0, _gameField.GetLength(1))
    .Select(j => _gameField[i, j] == null ? '.' : GetAnimalSymbol(_gameField[i, j])).ToArray())));

            Console.Clear();
            Console.WriteLine(gameState);

            if (_antelopeCount < 10)
            { await AddAnimalInitialSetup('A'); }
            else if (_lionCount < 10)
            { await AddAnimalInitialSetup('L'); }
            else
            {
                var animalsCopy = new List<IAnimal>(_gameLogic.GetAnimals);
                //Start Game: 
                foreach (var animal in animalsCopy)
                {
                    _gameLogic.PlayGame(animal);
                }
                string updatedGameState = _gameLogic.DrawField;
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

    private static char GetAnimalSymbol(IAnimal animal)
    {
        if (animal is Antelope)
        {
            return 'A';
        }
        else if (animal is Lion)
        {
            return 'L';
        }
        else
        {
            throw new ArgumentException("Unknown animal type");
        }
    }

    private static async Task AddAnimalInitialSetup(char animal)
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
            } while (_gameField[indexX, indexY] != null);
            IAnimal gameAnimal = null;
            if (animal == 'A')
            {
                gameAnimal = new Antelope();
                _antelopeCount++;
            }
            else if (animal == 'L')
            {
                gameAnimal = new Lion();
                _lionCount++;
            }

            if (gameAnimal != null)
            {
                gameAnimal.X = indexX;
                gameAnimal.Y = indexY;
                _gameField[indexX, indexY] = gameAnimal;
                _animalAdder.AddAnimal(gameAnimal);
            }
            else
            {
                throw new ArgumentException($"Invalid animal type: {animal}");
            }
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

    private static void DisplayAnimalHealth()
    {
        int antelopeId = 1;
        int lionId = 1;

        foreach (var animal in _gameLogic.GetAnimals)
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
    private static void DisplayLiveAnimalsCount()
    {
        var animals = _gameLogic.GetAnimals;
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
