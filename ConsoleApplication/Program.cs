using CodeLibrary;

namespace ConsoleApplication;

internal static class Program
{
    static void Main(string[] args)
    {
        var fieldSize = new FieldDisplayer.FieldSize(100, 40);
        var gameEngine = new GameEngine(fieldSize);
        var fieldDisplayer = new FieldDisplayer();
        int numberOfAntelopes = 0;
        ConsoleKey button;

        while (numberOfAntelopes < 2)
        {
            Console.WriteLine("Press 'A' to add an Antelope, 'S' to stop, when you have added enough of Antelopes");
            gameEngine.DrawField(fieldDisplayer);

            button = Console.ReadKey(true).Key;
            if (button == ConsoleKey.A)
            {
                gameEngine.AddAnimal(new CodeLibrary.Animals.Antelope());
                numberOfAntelopes++;
                Console.Clear();
                gameEngine.DrawField(fieldDisplayer);
            }
            else if (button == ConsoleKey.S)
            {
                Console.WriteLine("Hello");
                break;
            }
        }

        //Console.WriteLine("Press 'L' to add a Lion, 'S' to stop, when you have added enough of Lions");
        //int numberOfLions = 0;
        //while (numberOfLions < 10)
        //{
        //    var key = Console.ReadKey(true).Key;
        //    if (key == ConsoleKey.L)
        //    {
        //        gameEngine.AddAnimal(new CodeLibrary.Animals.Lion());
        //        numberOfLions++;
        //        Console.Clear();
        //        gameEngine.DrawField(fieldDisplayer);
        //    }
        //    else if (key == ConsoleKey.S && numberOfLions >= 2)
        //    {
        //        break;
        //    }
        //    Console.Clear();
        //    gameEngine.DrawField(fieldDisplayer);
        //}
    }

}
