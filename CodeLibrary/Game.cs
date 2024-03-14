using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class Game
{
    private char[,] gameField;
    private int antelopeCount;
    private int lionCount;
    private Random random;
    private readonly GameEngine gameEngine;
    private readonly FieldDisplayer fieldDisplayer;
    public Game()
    {
        gameField = new char[20, 100]; // Initialize the 2D array with the desired size
        for (int i = 0; i < gameField.GetLength(0); i++)
        {
            for (int j = 0; j < gameField.GetLength(1); j++)
            {
                gameField[i, j] = '.'; // Fill the game field with '.'
            }
        }
        antelopeCount = 0;
        lionCount = 0;
        random = new Random();
        fieldDisplayer = new FieldDisplayer();
        gameEngine = new GameEngine(new FieldDisplayer.FieldSize(20,100), fieldDisplayer);
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
            string gameState = string.Join("\n", Enumerable.Range(0, gameField.GetLength(0))
                .Select(i => new string(Enumerable.Range(0, gameField.GetLength(1))
                .Select(j => gameField[i, j]).ToArray())));

            Console.Clear(); // Clear the console
            Console.WriteLine(gameState);

            if (antelopeCount < 10)
            {
                await AddAnimalInitialSetup('A');
            }
            else if (lionCount < 10)
            {
                await AddAnimalInitialSetup('L');
            }
            else
            {
                // Create a copy of the animals list
                var animalsCopy = new List<IAnimal>(gameEngine.GetAnimals);
                //Start Game: 
                foreach (var animal in animalsCopy)
                {
                    gameEngine.MoveAnimal(animal);
                }

                // Display the updated state of the game field
                string updatedGameState = gameEngine.DrawField;
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

    /// <summary>
    /// ADD DESCRIPTION
    /// </summary>
    /// <param name="animal">The character representing the type of animal to be added ('A' for Antelope, 'L' for Lion).</param>
    private async Task AddAnimalInitialSetup(char animal)
    {
        if ((animal == 'A' && antelopeCount >= 10) || (animal == 'L' && lionCount >= 10))
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
                indexX = random.Next(gameField.GetLength(0));
                indexY = random.Next(gameField.GetLength(1));
            } while (gameField[indexX, indexY] != '.');
            gameField[indexX, indexY] = animal;
            if (animal == 'A')
            {
                antelopeCount++;
            }
            else if (animal == 'L')
            {
                lionCount++;
            }

            IAnimal gameAnimal = animal == 'A' ? new Antelope() : new Lion();
            gameAnimal.X = indexX;
            gameAnimal.Y = indexY;
            gameEngine.AddAnimal(gameAnimal);
        }
        else if (key == ConsoleKey.S && ((animal == 'A' && antelopeCount >= 2) || (animal == 'L' && lionCount >= 2)))
        {
            if (animal == 'A')
            {
                antelopeCount = 10;
            }
            else if (animal == 'L')
            {
                lionCount = 10;
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

        foreach (var animal in gameEngine.GetAnimals)
        {
            if (animal is Antelope && antelopeId <= antelopeCount)
            {
                Console.WriteLine(animal.Health > 0 ? $"Antelope {antelopeId}: health {animal.Health}" : $"Antelope {antelopeId}: health {animal.Health} - died");
                antelopeId++;
            }
            else if (animal is Lion && lionId <= lionCount)
            {
                Console.WriteLine(animal.Health > 0 ? $"Lion {lionId}: health {animal.Health}" : $"Lion {lionId}: health {animal.Health} -  died");
                lionId++;
            }
        }
    }
    private void DisplayLiveAnimalsCount()
    {
        var animals = gameEngine.GetAnimals;
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


