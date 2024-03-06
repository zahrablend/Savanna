using CodeLibrary;
using CodeLibrary.Animals;

namespace ConsoleApplication;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var fieldSize = new FieldDisplayer.FieldSize(100, 20);
        var gameEngine = new GameEngine(fieldSize);
        int antelopeCount = 0, lionCount = 0;
        bool addLion = false;
        bool gameOver = false;

        string[] gameStates = new string[3];

        while (!gameOver)
        {
            gameStates[2] = gameEngine.DrawField(new FieldDisplayer());
            Console.Clear(); // Clear the console
            Console.WriteLine(gameStates[2]);

            if (!addLion && antelopeCount <= 10)
            {
                Console.WriteLine("Add A to game field. Press A to continue or S to skip.");
                ConsoleKey key = await GetKeyPress();
                if (key == ConsoleKey.A)
                {
                    var antelope = new Antelope();
                    gameEngine.AddAnimal(antelope);
                    antelopeCount++;
                }
                else if (key == ConsoleKey.S && antelopeCount >= 2)
                {
                    addLion = true;
                }
                if (antelopeCount > 10)
                {
                    addLion = true;
                }
            }
            if (addLion && lionCount <= 10)
            {
                Console.WriteLine("Add L to game field. Press L to continue or S to skip.");
                ConsoleKey key = await GetKeyPress();
                if (key == ConsoleKey.L)
                {
                    var lion = new Lion();
                    gameEngine.AddAnimal(lion);
                    lionCount++;
                }
                else if (key == ConsoleKey.S && lionCount >= 2)
                {
                    gameOver = true;
                }
            }
            if (lionCount == 10)
            {
                gameOver = true;
            }
        }
        Console.WriteLine("Game Over");
        Console.ReadKey();
    }

    static async Task<ConsoleKey> GetKeyPress()
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
