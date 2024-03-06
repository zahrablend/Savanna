using CodeLibrary;
using CodeLibrary.Animals;

namespace ConsoleApplication;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var fieldSize = new FieldDisplayer.FieldSize(100, 40);
        var gameEngine = new GameEngine(fieldSize);
        int aCount = 0, lCount = 0;

        string[] gameStates = new string[3];

        while (true)
        {
            gameStates[2] = gameEngine.DrawField(new FieldDisplayer());
            Console.Clear(); // Clear the console
            Console.WriteLine(gameStates[2]);

            if (aCount < 3)
            {
                Console.WriteLine("Add A to game field. Press A to continue or S to skip.");
                ConsoleKey key = await GetKeyPress();
                if (key == ConsoleKey.A)
                {
                    var antelope = new Antelope();
                    gameEngine.AddAnimal(antelope);
                    aCount++;
                }
                else if (key == ConsoleKey.S)
                {
                    aCount = 3;
                }
            }
            else if (lCount < 3)
            {
                Console.WriteLine("Add L to game field. Press L to continue or S to skip.");
                ConsoleKey key = await GetKeyPress();
                if (key == ConsoleKey.L)
                {
                    var lion = new Lion();
                    gameEngine.AddAnimal(lion);
                    lCount++;
                }
                else if (key == ConsoleKey.S)
                {
                    lCount = 3;
                }
            }
            else
            {
                Console.WriteLine("Game Over");
                Console.ReadKey();
                break;
            }
        }
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
