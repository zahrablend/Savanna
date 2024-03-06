using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class Game
{
    private char[,] gameField;
    private int antelopeCount;
    private int lionCount;
    private Random random;
    private GameEngine gameEngine;

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
        gameEngine = new GameEngine(new FieldDisplayer.FieldSize(20,100));
    }

    private bool moveMade = false;
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

            if (!moveMade && antelopeCount < 10)
            {
                await AddAnimal('A');
            }
            else if (!moveMade && lionCount < 10)
            {
                await AddAnimal('L');
            }
            else
            {
                Console.WriteLine("Make a move");
                MoveAnimal();
                moveMade = true;
                Console.WriteLine(gameEngine.DrawField(new FieldDisplayer()));
                Console.WriteLine("Make a move");
                MoveAnimal();
                Console.WriteLine(gameEngine.DrawField(new FieldDisplayer()));
            }
        }
    }

    private void MoveAnimal()
    {
        IAnimal[,] currentGameField = gameEngine.GetGameField();
        foreach (var animal in gameEngine.GetAnimals())
        {
            if (animal != null)
            {
                // Call the MoveAnimal method of the animal
                animal.MoveAnimal(currentGameField, gameField.GetLength(0), gameField.GetLength(1));

                // Update the gameField in the Game class
                gameField[animal.X, animal.Y] = animal is Antelope ? 'A' : 'L';
            }
        }
        gameEngine.UpdateGameField(currentGameField);
    }

    private async Task AddAnimal(char animal)
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


    private async Task<ConsoleKey> GetKeyPress()
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
}


