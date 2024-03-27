using CodeLibrary.Constants;
using CodeLibrary.GameEngine;
using Common.Interfaces;

namespace CodeLibrary;

public class Game
{
    private char[,] _gameField;
    private Random _random;
    private readonly GameLogicOrchestrator _logic;
    private readonly FieldDisplayer _fieldDisplayer;
    private readonly IGameUI _gameUI;
    private readonly Dictionary<char, (IAnimalFactory factory, int count)> _animalFactories;
    private readonly Queue<char> _animalOrder;
    //private readonly IGameRepository _gameRepository;


    public Game(IGameUI gameUI)
    {
        _gameUI = gameUI;
        _gameField = new char[20, 100];
        for (int i = 0; i < _gameField.GetLength(0); i++)
        {
            for (int j = 0; j < _gameField.GetLength(1); j++)
            {
                _gameField[i, j] = '.';
            }
        }
        _random = new Random();
        _fieldDisplayer = new FieldDisplayer();
        _fieldDisplayer.Size = new FieldDisplayer.FieldSize(20, 100);
        _logic = new GameLogicOrchestrator(_fieldDisplayer);
        var antelopeAssemblyPath = Environment.GetEnvironmentVariable("ANTELOPEBEHAVIOUR_PATH");
        var lionAssemblyPath = Environment.GetEnvironmentVariable("LIONBEHAVIOUR_PATH");

        if (string.IsNullOrEmpty(antelopeAssemblyPath) || string.IsNullOrEmpty(lionAssemblyPath))
        {
            throw new InvalidOperationException("Environment variables ANTELOPEBEHAVIOUR_PATH and/or LIONBEHAVIOUR_PATH are not set.");
        }

        var antelopeAssembly = AssemblyLoader.LoadAssembly(antelopeAssemblyPath);
        var lionAssembly = AssemblyLoader.LoadAssembly(lionAssemblyPath);

        // Get the AntelopeFactory and LionFactory types
        var antelopeFactoryType = antelopeAssembly.GetType("AntelopeBehaviour.AntelopeFactory");
        var lionFactoryType = lionAssembly.GetType("LionBehaviour.LionFactory");

        if (antelopeFactoryType == null || lionFactoryType == null)
        {
            throw new TypeLoadException("Type not found in assembly.");
        }

        // Create instances of the AntelopeFactory and LionFactory
        var antelopeFactory = (IAnimalFactory)Activator.CreateInstance(antelopeFactoryType);
        var lionFactory = (IAnimalFactory)Activator.CreateInstance(lionFactoryType);

        // Initialize the animal factories
        _animalFactories = new Dictionary<char, (IAnimalFactory factory, int count)>
        {
            { antelopeFactory.Symbol, (antelopeFactory, 0) },
            { lionFactory.Symbol, (lionFactory, 0) }
        };

        _animalOrder = new Queue<char>();
        _animalOrder.Enqueue(antelopeFactory.Symbol);
        _animalOrder.Enqueue(lionFactory.Symbol);
        //_gameRepository = gameRepository;
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
            string gameState = string.Join("\n", Enumerable.Range(0, _gameField.GetLength(0))
        .Select(i => new string(Enumerable.Range(0, _gameField.GetLength(1))
        .Select(j => _gameField[i, j]).ToArray())));

            _gameUI.Clear();
            _gameUI.Display(gameState);

            if (_animalOrder.Count > 0)
            {
                await AddAnimalInitialSetup();
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
                _gameUI.Clear();
                _gameUI.Display(updatedGameState);
                DisplayAnimalHealth();
                await Task.Delay(100);

                var key = await _gameUI.GetKeyPress();
                if (key.HasValue && key.Value == ConsoleKey.S)
                {
                    _gameUI.Display(Constant.GameOverMessage);
                    DisplayLiveAnimalsCount();
                    _gameUI.Display(Constant.CloseAppMessage);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Asynchronously adds an animal to the game field during the initial setup.
    /// The user can choose to add the animal by pressing a key, or skip to the next step by pressing 'S'.
    /// </summary>
    /// <param name="animal">The character representing the type of animal to add ('A' for Antelope, 'L' for Lion).</param>
    private async Task<ConsoleKey?> AddAnimalInitialSetup()
    {
        if (_animalOrder.Count == 0)
        {
            return null;
        }

        char animalSymbol = _animalOrder.Peek();
        var (animalFactory, count) = _animalFactories[animalSymbol];

        if (count >= 10)
        {
            _animalOrder.Dequeue();
            return await AddAnimalInitialSetup();
        }

        _gameUI.Display($"Add {animalSymbol} to game field. Press {animalSymbol} to continue or S to skip.");
        ConsoleKey? key = await _gameUI.GetKeyPress();
        if (key.HasValue && key.Value.ToString().ToUpper() == animalSymbol.ToString())
        {
            int indexX, indexY;
            do
            {
                indexX = _random.Next(_gameField.GetLength(0));
                indexY = _random.Next(_gameField.GetLength(1));
            } while (_gameField[indexX, indexY] != '.');
           
            _gameField[indexX, indexY] = animalSymbol;

            IAnimal gameAnimal = animalFactory.Create();
            gameAnimal.X = indexX;
            gameAnimal.Y = indexY;
            _logic.AddAnimal(gameAnimal);

            _animalFactories[animalSymbol] = (animalFactory, count + 1);
        }
        else if (key.HasValue && key.Value == ConsoleKey.S && count >= 2)
        {
            _animalOrder.Dequeue();
        }
        return key;
    }

    /// <summary>
    /// Displays the health status of each Antelope and Lion in the game.
    /// For each animal, it prints the animal's type, ID, and current health.
    /// If an animal's health is zero or less, it also prints that the animal has died.
    /// </summary>
    private void DisplayAnimalHealth()
    {
        var animalId = new Dictionary<string, int>();

        foreach (var animal in _logic.GetAnimals)
        {
            if (!animalId.TryGetValue(animal.Name, out int value))
            {
                value = 1;
                animalId[animal.Name] = value;
            }
            _gameUI.Display(animal.Health > 0 
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

        foreach (var animal in _logic.GetAnimals)
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
            _gameUI.Display($"Live {animal}s: {liveAnimals[animal]}");
        }

        if (liveAnimals.Count == 1)
        {
            _gameUI.Display($"{liveAnimals.Keys.First()}s won");
        }
    }
}


