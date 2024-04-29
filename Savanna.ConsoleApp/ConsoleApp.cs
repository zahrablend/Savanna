using CodeLibrary.GameEngine;
using CodeLibrary;
using Common.Interfaces;
using Common.ValueObjects;
using Savanna.Infrastructure;
using Common.Constants;

namespace Savanna.ConsoleApp;

public class ConsoleApp
{
    private AnimalFactoryLoader _animalFactoryLoader;
    private IGameField _gameField;
    private FieldDisplayer _fieldDisplayer;
    private IGameUI _consoleGameUI;
    private Random _random;
    private GameSetup _gameSetup;
    private AnimalDictionary _animalDict;
    private Queue<string> _animalOrder;
    private GameLogicOrchestrator _gameLogicOrchestrator;

    public ConsoleApp(FieldDisplayer fieldDisplayer, AnimalFactoryLoader animalFactoryLoader, IGameUI gameUI, IGameField gameField, IGameUI consoleGameUI)
    {
        _animalFactoryLoader = animalFactoryLoader;
        _fieldDisplayer = fieldDisplayer;
        _consoleGameUI = consoleGameUI;
        _random = new Random();
        _animalDict = new AnimalDictionary();
        _animalOrder = new Queue<string>();
        _gameSetup = new GameSetup(gameField, _animalDict);

        string antelopeBehaviourPath = Environment.GetEnvironmentVariable("ANTELOPEBEHAVIOUR_PATH");
        var antelopeFactory = _animalFactoryLoader.LoadAnimalFactory(antelopeBehaviourPath, "AntelopeBehaviour.AntelopeFactory");
        _animalDict.RepresentAnimal(antelopeFactory);
        _gameSetup.AddAnimalFactory(antelopeFactory);
        _animalOrder.Enqueue(antelopeFactory.Species);

        string lionBehaviourPath = Environment.GetEnvironmentVariable("LIONBEHAVIOUR_PATH");
        var lionFactory = _animalFactoryLoader.LoadAnimalFactory(lionBehaviourPath, "LionBehaviour.LionFactory");
        _animalDict.RepresentAnimal(lionFactory);
        _gameSetup.AddAnimalFactory(lionFactory);
        _animalOrder.Enqueue(lionFactory.Species);
    }

    public async Task RunGame()
    {
        await Run();
    }

    public async Task Run()
    {
        // Draw the current state of the game field
        var emptyCell = new FieldCell { State = null };
        _gameSetup.GameField.Initialize(emptyCell);

        while (true)
        {
            string gameState = _fieldDisplayer.DrawField(_gameSetup.GameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width, GameSetup.DisplayType.Symbol);
            Console.Clear();
            _consoleGameUI.Display(gameState);
            if (_animalOrder.Count > 0)
            {
                // Add animals to the game field until they're all added
                await AddAnimalInitialSetup();
            }
            else
            {
                // Start the game
                foreach (var animal in _gameSetup.GetAnimals())
                {
                    _gameLogicOrchestrator.PlayGame(animal);
                }

                // Draw the updated state of the game field
                string updatedGamestate = _fieldDisplayer.DrawField(_gameSetup.GameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width, GameSetup.DisplayType.Symbol);
                Console.Clear();
                _consoleGameUI.Display(updatedGamestate);

                // Check for game over condition
                var key = await _consoleGameUI.GetKeyPress();
                if (key.HasValue && key.Value == ConsoleKey.S)
                {
                    _consoleGameUI.Display(Constant.GameOverMessage);
                    _consoleGameUI.Display(Constant.CloseAppMessage);
                    break;
                }
            }
        }
    }


    private async Task<ConsoleKey?> AddAnimalInitialSetup()
    {
        if (_animalOrder.Count == 0)
        {
            return null;
        }
        string animalSpecies = _animalOrder.Peek();
        IAnimalFactory animalFactory = _gameSetup.GetAnimalFactoryBySpecies(animalSpecies);
        _gameSetup.AddAnimal(animalFactory);
        var count = _gameSetup.GetAnimalCount(animalSpecies);
        if (count >= 10)
        {
            _animalOrder.Dequeue();
            return await AddAnimalInitialSetup();
        }
        _consoleGameUI.Display($"Add {animalSpecies} to game field. Press first letter of Species to continue or S to skip.");
        ConsoleKey? key = await _consoleGameUI.GetKeyPress();
        if (key.HasValue && key.Value.ToString().ToUpper() == animalSpecies.ToString().ToUpper())
        {
            int indexX, indexY;
            do
            {
                indexX = _random.Next(_gameField.Width);
                indexY = _random.Next(_gameField.Height);
            } while (!_gameField.GetState(indexX, indexY).IsEmpty);

            IAnimal gameAnimal = animalFactory.Create();
            gameAnimal.X = indexX;
            gameAnimal.Y = indexY;
            _gameField.SetState(indexX, indexY, new FieldCell { State = gameAnimal });

            _gameSetup.AddAnimal(animalFactory);

            string updatedGameState = _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width, GameSetup.DisplayType.Symbol);
            Console.Clear();
            string animalRepresentation = _gameSetup.DisplayAnimalRepresentation(gameAnimal, GameSetup.DisplayType.Symbol);
            _consoleGameUI.Display($"{animalRepresentation} {updatedGameState}");
        }
        else if (key.HasValue && key.Value == ConsoleKey.S && count >= 2)
        {
            _animalOrder.Dequeue();
        }
        return key;
    }
}
