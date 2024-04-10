using CodeLibrary.GameEngine;
using Common.Interfaces;
using Savanna.Infrastructure;

namespace CodeLibrary;

public class Game
{
    private IGameField _gameField;
    private Random _random;
    private readonly GameLogicOrchestrator _logic;
    private FieldDisplayer _fieldDisplayer;
    private readonly IGameUI _gameUI;
    private readonly Dictionary<char, (IAnimalFactory factory, int count)> _animalFactories;
    private readonly Queue<char> _animalOrder;
    private IGameRunner _gameRunner;
    private readonly AnimalFactoryLoader _animalFactoryLoader;
    private readonly GameSetup _gameSetup;
    private string _gameState;
    private readonly IGameRunningCallback _gameRunningCallback;

    public Dictionary<char, (IAnimalFactory factory, int count)> AnimalFactories => _animalFactories;
    public GameSetup GameSetup => _gameSetup;

    public FieldDisplayer FieldDisplayer { get; private set; }
    public int GameId { get; set; }
    public string GameState { get; set; }
    public string Name { get; set; }
    public int GameIteration { get; set; }

    public IGameField GameField => _gameField;
    public GameLogicOrchestrator Logic => _logic;
    public IGameUI GameUI => _gameUI;
    public Queue<char> AnimalOrder => _animalOrder;

    public Game(IGameUI gameUI, IGameRunner gameRunner, IGameFieldFactory gameFieldFactory, AnimalFactoryLoader animalFactoryLoader, GameSetup gameSetup, IGameRunningCallback gameRunningCallback = null)
    {
        _gameUI = gameUI;
        _gameRunner = gameRunner;
        _gameField = gameFieldFactory.Create(20, 100);
        _gameField.Initialize('.');
        _random = new Random();
        _fieldDisplayer = new FieldDisplayer();
        _fieldDisplayer.Size = new FieldDisplayer.FieldSize(20, 100);
        _logic = new GameLogicOrchestrator(_fieldDisplayer, gameFieldFactory, gameSetup);
        _animalFactoryLoader = animalFactoryLoader;
        _gameSetup = gameSetup;

        var antelopeFactory = _animalFactoryLoader.LoadAnimalFactory(
            Environment.GetEnvironmentVariable("ANTELOPEBEHAVIOUR_PATH"),
            "AntelopeBehaviour.AntelopeFactory"
        );

        var lionFactory = _animalFactoryLoader.LoadAnimalFactory(
            Environment.GetEnvironmentVariable("LIONBEHAVIOUR_PATH"),
            "LionBehaviour.LionFactory"
        );

        _animalFactories = new Dictionary<char, (IAnimalFactory factory, int count)>
        {
            { antelopeFactory.Symbol, (antelopeFactory, 0) },
            { lionFactory.Symbol, (lionFactory, 0) }
        };

        _animalOrder = new Queue<char>();
        _animalOrder.Enqueue(antelopeFactory.Symbol);
        _animalOrder.Enqueue(lionFactory.Symbol);
        _gameRunningCallback = gameRunningCallback;
        _gameRunner.SetGameRunningCallback(_gameRunningCallback);
    }


    /// <summary>
    /// Asynchronously adds an animal to the game field during the initial setup.
    /// The user can choose to add the animal by pressing a key, or skip to the next step by pressing 'S'.
    /// </summary>
    /// <param name="animal">The character representing the type of animal to add ('A' for Antelope, 'L' for Lion).</param>
    public async Task<ConsoleKey?> AddAnimalInitialSetup()
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
        if (key.HasValue && key.Value.ToString().ToUpper() == animalSymbol.ToString().ToUpper())
        {
            int indexX, indexY;
            do
            {
                indexX = _random.Next(20);
                indexY = _random.Next(100);
            } while ((char)_gameField.GetState(indexX, indexY) != '.');

            IAnimal gameAnimal = animalFactory.Create();
            gameAnimal.X = indexX;
            gameAnimal.Y = indexY;
            _gameField.SetState(indexX, indexY, gameAnimal);
            _gameSetup.AddAnimal(gameAnimal);
            string updatedGameState = _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
            _gameUI.Clear();
            _gameUI.Display(updatedGameState);
            _animalFactories[animalSymbol] = (animalFactory, count + 1);
        }
        else if (key.HasValue && key.Value == ConsoleKey.S && count >= 2)
        {
            _animalOrder.Dequeue();
        }
        return key;
    }

    public async Task Run()
    {
        await _gameRunner.Run();
    }

    public void UpdateGameState()
    {
        _gameState = _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
    }

    public List<char> GetAnimalTypes()
    {
        return _animalFactories.Keys.ToList();
    }

    public void SetFieldDisplayerSize(int height, int width)
    {
        _fieldDisplayer.Size = new FieldDisplayer.FieldSize(height, width);
    }

}


