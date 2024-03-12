using CodeLibrary;
using CodeLibrary.Animals;

namespace ConsoleApplication;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var game = new Game();
        await game.Run();

        // After running the game, you can call the test method
        TestBirthMethod();
        Console.ReadLine();
    }

    private static void TestBirthMethod()
    {
        // Initialize the game
        var gameEngine = new GameEngine(new FieldDisplayer.FieldSize(20, 100), new FieldDisplayer());
        var animal1 = new Antelope { X = 0, Y = 0 };
        var animal2 = new Antelope { X = 0, Y = 1 };
        gameEngine.AddAnimal(animal1);
        gameEngine.AddAnimal(animal2);

        // Simulate the game
        for (int i = 0; i < 3; i++)
        {
            gameEngine.MoveAnimal(animal1);
            gameEngine.MoveAnimal(animal2);
        }

        // Check the result
        Console.WriteLine(gameEngine.GetAnimals().Count);  // Should print 3 if the Birth method works correctly
    }
}
