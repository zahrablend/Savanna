internal static class Program
{
    static async Task Main(string[] args)
    {
        char[] gameField = Enumerable.Repeat('.', 8).ToArray();
        int aCount = 0, lCount = 0;
        Random random = new Random();

        string[] gameStates = new string[3];

        while (true)
        {
            //gameStates[0] = "Iteration: " + (aCount + lCount).ToString();
            //gameStates[1] = "Live Cells: " + (aCount + lCount).ToString();
            gameStates[2] = new string(gameField);

            Console.Clear(); // Clear the console
            //Console.WriteLine(gameStates[0]);
            //Console.WriteLine(gameStates[1]);
            Console.WriteLine(gameStates[2]);

            if (aCount < 3)
            {
                Console.WriteLine("Add A to game field. Press A to continue or S to skip.");
                ConsoleKey key = await GetKeyPress();
                if (key == ConsoleKey.A)
                {
                    int index;
                    do
                    {
                        index = random.Next(gameField.Length);
                    } while (gameField[index] != '.');
                    gameField[index] = 'A';
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
                    int index;
                    do
                    {
                        index = random.Next(gameField.Length);
                    } while (gameField[index] != '.');
                    gameField[index] = 'L';
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

/*
 if (aCount >= 3 && lCount >= 3)
{
    Console.WriteLine("Game Over");
    Console.ReadKey();
    break;
}

 */