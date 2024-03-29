using CodeLibrary.Interfaces;
using CodeLibrary;

namespace Savanna.ConsoleApp
{
    public class ConsoleGameRunner : IGameRunner
    {
        public async Task Run(Game game)
        {
            while (game.IsRunning)
            {
                await game.Run();
            }
        }
    }
}
