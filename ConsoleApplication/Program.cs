using CodeLibrary;

namespace ConsoleApplication;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var game = new Game();
        await game.Run();
    }
}
